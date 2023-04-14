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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;

class EncryptionTest
{
    byte[][] plainDatas =
    {
        Encoding.UTF8.GetBytes("AAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
        Encoding.UTF8.GetBytes("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB"),
        Encoding.UTF8.GetBytes("CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC"),
    };

    int cnt = 1000;

    public EncryptionTest()
    {
        for (int i = 0; i < plainDatas.Length; i++)
        {
            //plainDatas[i] =  new byte[640 * 360];
            plainDatas[i] = BlowfishCipher.MakeRandomBytes(512);
        }
        //var t = new Thread[100];
        //while (true)
        //{
        //    Stopwatch st = Stopwatch.StartNew();
        //    for (int i = 0; i < 100; i++)
        //    {
        //        t[i] = new Thread(AES);
        //        t[i].Start();
        //    }
        //    for (int i = 0; i < 100; i++)
        //    {
        //        t[i].Join();
        //    }
        //    st.Stop();
        //    Console.WriteLine("BF " + st.ElapsedMilliseconds);
        //}
        AES();
    }

    private void BF()
    {
        byte[] serverKey = BlowfishCipher.MakeRandomKey();
        byte[] clientKey = BlowfishCipher.MakeRandomIV();

        var enc = new BlowfishCipher(serverKey);
        enc.IV = clientKey;
        var dec = new BlowfishCipher(serverKey);
        dec.IV = clientKey;

        byte[] ori = new byte[640 * 360 * 2];
        byte[] en = new byte[640 * 360 * 2];
        byte[] de = new byte[640 * 360 * 2];

        for (int c = 0; c < cnt; c++)
        {
            foreach (var data in plainDatas)
            {
                Buffer.BlockCopy(data, 0, ori, 0, data.Length);
                int dataLen = data.Length % 16 == 0 ? data.Length : data.Length + 16 - (data.Length % 16);
                enc.Encrypt(ori, 0, dataLen, ref en, out var elen);
                dec.Decrypt(en, 0, elen, ref de, out var dlen);

                //for (int i = 0; i < data.Length; i++)
                //{
                //    if (data[i] != de[i])
                //    {
                //        Console.WriteLine("Error!");
                //        Console.ReadLine();
                //    }
                //}
            }
        }
    }

    private void AES()
    {
        byte[] key = BlowfishCipher.MakeRandomBytes(32);
        byte[] iv = BlowfishCipher.MakeRandomBytes(16);

        var aesAlg = Aes.Create();
        aesAlg.KeySize = key.Length * 8;
        aesAlg.BlockSize = iv.Length * 8;
        aesAlg.Key = new byte[32];
        //aesAlg.IV = new byte[16];
        aesAlg.Padding = PaddingMode.None;
        var enc = aesAlg.CreateEncryptor();
        var dec = aesAlg.CreateDecryptor();

        byte[] ori = new byte[640 * 360 * 2];
        byte[] en = new byte[640 * 360 * 2];
        byte[] de = new byte[640 * 360 * 2];

        //Stopwatch st = Stopwatch.StartNew();
        for (int c = 0; c < cnt; c++)
        {
            foreach (var data in plainDatas)
            {
                //Buffer.BlockCopy(data, 0, ori, 0, data.Length);
                int dataLen = data.Length % 16 == 0 ? data.Length : data.Length + 16 - (data.Length % 16);
                byte[] data2 = new byte[dataLen];
                Buffer.BlockCopy(data, 0, data2, 0, data2.Length);

                int elen = enc.TransformBlock(data2, 0, dataLen, data2, 0);
                int dlen = dec.TransformBlock(data2, 0, elen, data2, 0);

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != de[i])
                    {
                        Console.WriteLine("Error!");
                        Console.ReadLine();
                    }
                }
            }
        }
        //st.Stop();
        //Console.WriteLine("AES " + st.ElapsedMilliseconds);
    }

    private void AES2()
    {
        byte[] key = BlowfishCipher.MakeRandomBytes(32);
        byte[] iv = BlowfishCipher.MakeRandomBytes(16);

        var aesAlg = Aes.Create();
        aesAlg.KeySize = key.Length * 8;
        aesAlg.BlockSize = iv.Length * 8;
        aesAlg.Key = key;
        aesAlg.IV = iv;
        aesAlg.Padding = PaddingMode.None;
        var enc = aesAlg.CreateEncryptor();
        var dec = aesAlg.CreateDecryptor();

        byte[] eA = Trans(enc, plainDatas[0]);
        byte[] eB = Trans(enc, plainDatas[1]);
        byte[] eC = Trans(enc, plainDatas[2]);

        aesAlg = Aes.Create();
        aesAlg.KeySize = key.Length * 8;
        aesAlg.BlockSize = iv.Length * 8;
        aesAlg.Key = key;
        aesAlg.IV = new byte[16];
        aesAlg.Padding = PaddingMode.None;
        enc = aesAlg.CreateEncryptor();
        dec = aesAlg.CreateDecryptor();

        byte[] dA = Trans(dec, eA).Take(plainDatas[0].Length).ToArray();
        byte[] dB = Trans(dec, eB).Take(plainDatas[1].Length).ToArray();
        byte[] dC = Trans(dec, eC).Take(plainDatas[2].Length).ToArray();

        string A = Encoding.UTF8.GetString(dA);
        string B = Encoding.UTF8.GetString(dB);
        string C = Encoding.UTF8.GetString(dC);
    }


    public byte[] Trans(ICryptoTransform t, byte[] data)
    {
        int dataLen = data.Length % 16 == 0 ? data.Length : data.Length + 16 - (data.Length % 16);
        byte[] ori = new byte[dataLen];
        byte[] rec = new byte[dataLen * 2];
        Buffer.BlockCopy(data, 0, ori, 0, data.Length);
        int elen = t.TransformBlock(ori, 0, dataLen, rec, 0);
        byte[] rst = new byte[elen];
        Buffer.BlockCopy(rec, 0, rst, 0, elen);
        return rst;
    }
}