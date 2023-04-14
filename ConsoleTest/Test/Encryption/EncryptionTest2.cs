//Blowfish encryption (ECB and CBC MODE) as defined by Bruce Schneier here: http://www.schneier.com/paper-blowfish-fse.html
//Complies with test vectors found here: http://www.schneier.com/code/vectors.txt
//non-standard mode profided to be usable with the javascript crypto library found here: http://etherhack.co.uk/symmetric/blowfish/blowfish.html
//By FireXware, 1/7/1010, Contact: firexware@hotmail.com
//Code is partly adopted from the javascript crypto library by Daniel Rench


//USAGE:
//BlowFish b = new BlowFish("04B915BA43FEB5B6");
//string plainText = "The quick brown fox jumped over the lazy dog.";
//string cipherText = b.Encrypt_CBC(plainText);
//MessageBox.Show(cipherText);
//plainText = b.Decrypt_CBC(cipherText);
//MessageBox.Show(plainText);

using System;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Realwith.Encr
{
    public class EncryptionTest2
    {
        public EncryptionTest2()
        {
            byte[] serverKey12byte = new byte[] { (byte)165, (byte)210, (byte)94, (byte)236, (byte)121, (byte)42, (byte)127, (byte)39 };
            byte[] clientKey12byte = new byte[] { (byte)2, (byte)96, (byte)217, (byte)63, (byte)187, (byte)62, (byte)207, (byte)205 };
            byte[] seg = new byte[] { (byte)32, (byte)0, (byte)0, (byte)2, (byte)0, (byte)64, (byte)212, (byte)0, (byte)1, (byte)64, (byte)35, (byte)255, (byte)43, (byte)225, (byte)44, (byte)116, (byte)81, (byte)207, (byte)5, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0 };
            
            var e = new Encryption(serverKey12byte, clientKey12byte);

            byte[] data = new byte[32];
            int esize = e.Encrypt(seg, 3, 32, ref data);
        }
    }
    public class Encryption : IDisposable
    {
        static RNGCryptoServiceProvider randomSource;
        static Encryption()
        {
            randomSource = new RNGCryptoServiceProvider();
        }

        public static byte[] MakeRandomBytes(int size)
        {
            var k = new byte[size];
            randomSource.GetBytes(k);
            return k;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    MEMBERS
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public Socket TargetSocket { get; set; }
        public string Name { get; private set; }
        public int ReceivePort { get; private set; }
        public int ClientVersion { get; private set; }
        public bool IsRun { get; private set; }
        public bool IsRunReceiver { get; private set; }
        public bool IsRunSender { get; private set; }

        private Aes aes;
        private byte[] Key = new byte[]
        {
            0x5C, 0x4F, 0xD5, 0xCF, 0x58, 0x63, 0xC5, 0x03, // FIXED KEY 8 BYTES
            0xE1, 0xF0, 0x0F, 0x03, 0x7A, 0x1C, 0x33, 0x01, // FIXED KEY 8 BYTES
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // SERVER KEYS
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // CLIENT KEYS
        };

        private ICryptoTransform encryptor;
        private ICryptoTransform decryptor;

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    LIFECYCLE
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public Encryption(byte[] serverKey12byte, byte[] clientKey12byte)
        {
            ApplyKeys(serverKey12byte, clientKey12byte);

            aes = Aes.Create();
            aes.Padding = PaddingMode.None;
            aes.Key = Key;
            aes.IV = new byte[16];

            encryptor = aes.CreateEncryptor();
            decryptor = aes.CreateDecryptor();
        }

        private void ApplyKeys(byte[] serverKey12byte, byte[] clientKey12byte)
        {
            for (int i = 0; i < 8; i++)
            {
                Key[0x10 + i] = serverKey12byte[i];
                Key[0x18 + i] = clientKey12byte[i];
            }
        }

        public int Encrypt(byte[] src, int srcOffset, int srcLength, ref byte[] dst)
        {
            srcLength = PaddedLength(srcLength);
            int dstLength = PaddedLength(srcLength);

            if (src.Length < srcOffset + srcLength)
            {
                byte[] temp = new byte[srcLength];
                Buffer.BlockCopy(src, srcOffset, temp, 0, src.Length);
                src = temp;
                srcOffset = 0;
            }

            if (dst == null || dst.Length < dstLength)
            {
                dst = new byte[dstLength];
            }

            lock (encryptor)
            {
                return encryptor.TransformBlock(src, srcOffset, srcLength, dst, 0);
            }
        }

        public int Decrypt(byte[] src, int srcOffset, int srcLength, ref byte[] dst)
        {
            srcLength = PaddedLength(srcLength);
            int dstLength = PaddedLength(srcLength);

            if (src.Length < srcOffset + srcLength)
            {
                byte[] temp = new byte[srcLength];
                Buffer.BlockCopy(src, srcOffset, temp, 0, src.Length);
                src = temp;
                srcOffset = 0;
            }

            if (dst == null || dst.Length < dstLength)
            {
                dst = new byte[dstLength];
            }

            lock (decryptor)
            {
                return decryptor.TransformBlock(src, srcOffset, srcLength, dst, dstLength);
            }
        }

        public static int PaddedLength(int i)
        {
            if (i % 16 == 0) return i;
            return i + 16 - (i % 16);
        }

        public void Dispose()
        {
            aes?.Dispose();
        }
    }
}
