using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleTest.Test
{
    public class RSATest
    {
        public RSATest(string[] args)
        {
            try
            {
                //Create a UnicodeEncoder to convert between byte array and string.
                var ByteConverter = Encoding.UTF8;

                //Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] dataToEncrypt = File.ReadAllBytes("E:/Desktop.zip"); // ByteConverter.GetBytes("Data to Encrypt");
                byte[] encryptedData;
                byte[] decryptedData;
                string privateKeyText;
                string publicKeyText;

                Stopwatch st = Stopwatch.StartNew();
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // 복호화용
                    var privateKey = RSA.Create().ExportParameters(true);
                    rsa.ImportParameters(privateKey);
                    privateKeyText = rsa.ToXmlString(true);

                    // 공개키 생성 (암호화용)
                    var publicKey = new RSAParameters();
                    publicKey.Modulus = privateKey.Modulus;
                    publicKey.Exponent = privateKey.Exponent;
                    rsa.ImportParameters(publicKey);
                    publicKeyText = rsa.ToXmlString(false);

                    encryptedData = Encrypt(dataToEncrypt, publicKeyText, false);
                }

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    decryptedData = RSADecrypt(encryptedData, privateKeyText, false);
                    Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
                }
                st.Stop();
                Console.WriteLine(st.ElapsedMilliseconds);
            }
            catch (ArgumentNullException)
            {
                //Catch this exception in case the encryption did
                //not succeed.
                Console.WriteLine("Encryption failed.");
            }
        }

        private void ApplyBytes(byte[] dst, byte[] key)
        {
            for(int i=0; i<dst.Length; i++)
            {
                dst[i] = key[i % key.Length];
            }
        }

        public byte[] Encrypt(byte[] DataToEncrypt, string key, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(key);

                    encryptedData = rsa.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public byte[] RSADecrypt(byte[] DataToDecrypt, string key, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.FromXmlString(key);

                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }
    }
}
