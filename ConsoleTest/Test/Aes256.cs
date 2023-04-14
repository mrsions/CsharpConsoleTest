using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleTest
{
	// 안녕 UTF8 BOM
    public class Aes256
    {
        const string PRIVATE_KEY = "KnC2#wk4T7&X*@bzvg5GJW#4udGF*h32=2Y9J3TM4X-k2mZWtp%M+5^79&wT!NA2uva#^e%U?7qmy*YJ=Tvq3U+z94R5qtghynfa^kbnbBK-Qek#ymE3n3x_%k@Svgr8!^qVz3!Jv=@qFbm^MHskE6bx+tAbgMU$uT?=T3=*K6YvE_s+&j$j+xC7yDBzQYf^j9MSbt%39RP!!nVqHwhC5=fhg@zL^*24W_&*rn!3h&3L%VAcYYWbH36j#PypWyf6";
        public static void Test()
        {
            Test("a");
            Test("b");
            Test("c");
            Test("d");
        }
        private static void Test(string v)
        {
            var aes = new Aes256(PRIVATE_KEY, v);
            var plane = "안녕하세요. abcdefg-0123456789!@#$%^&*()_+";
            string enc = aes.EncryptToBase64(plane);
            string dec = aes.DecryptToString(enc);
            if (plane == dec)
            {
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("password : " + v);
                Console.WriteLine("enc      : " + enc);
            }
            else
            {
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("ERROR");
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    STATIC
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 두 문자를 섞은 값을 반환합니다.
        /// 단순히 더하는 것이 아닌 균일하게 두 문자를 분포시킵니다.
        /// a와 b 둘 사이의 길이는 상관없습니다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <example>
        /// MergeString("012345678", "abcdefghi") => "0a1b2c3d4e5f6g7h8i"
        /// MergeString("012345678", "abcde") => "0a1b2c3d4e5678"
        /// MergeString("012345678", "a") => "0123a45678"
        /// </example>
        public static string MergeString(string a, string b)
        {
            // a가 b보다 커야한다
            if (a.Length < b.Length)
            {
                var temp = a;
                a = b;
                b = temp;
            }

            StringBuilder sb = new StringBuilder(a.Length + b.Length);
            int insert = a.Length / b.Length;
            int start = 0;
            int length = 0;
            for (int i = 0; i < b.Length; i++)
            {
                length = insert;
                if (i == 0 && length >= 2) length /= 2;
                if (start + length > a.Length)
                {
                    length = a.Length - start;
                }
                if (length > 0)
                {
                    sb.Append(a.Substring(start, length));
                }
                sb.Append(b[i]);
                start += length;
            }

            if (start < a.Length)
            {
                length = a.Length - start;
                if (length > 0)
                {
                    sb.Append(a.Substring(start, length));
                }
            }

            return sb.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    VARIABLES
        //
        ///////////////////////////////////////////////////////////////////////////////////////
        
        private SHA256Managed sha256Managed = new SHA256Managed();
        private RijndaelManaged aes = new RijndaelManaged();
        private byte[] secretKey;
        private byte[] iv;

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    CONSTRUCTOR
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public Aes256(string password)
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            SetPassword(password);
        }

        public Aes256(string privatePassword, string publicPassword)
            : this(MergeString(privatePassword, publicPassword))
        { }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    ACTION
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public void SetPassword(string password)
        {
            // Salt는 비밀번호의 길이를 SHA256 해쉬값으로 한다.
            var salt = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(password.Length.ToString()));

            //PBKDF2(Password-Based Key Derivation Function)
            //반복은 65535번
            var PBKDF2Key = new Rfc2898DeriveBytes(password, salt, 0xFFFF, HashAlgorithmName.SHA256);
            secretKey = PBKDF2Key.GetBytes(aes.KeySize / 8);
            iv = PBKDF2Key.GetBytes(aes.BlockSize / 8);
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    ENCRYPT
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public string EncryptToBase64(string planeText) => Convert.ToBase64String(Encrypt(planeText));
        public string EncryptToBase64(byte[] planeData) => Convert.ToBase64String(Encrypt(planeData));
        public byte[] Encrypt(string planeText) => Encrypt(Encoding.UTF8.GetBytes(planeText));
        public byte[] Encrypt(byte[] planeData)
        {
            if (secretKey == null) throw new KeyNotFoundException();

            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(secretKey, iv), CryptoStreamMode.Write))
                {
                    cs.Write(planeData, 0, planeData.Length);
                }
                xBuff = ms.ToArray();
            }
            return xBuff;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    DECRYPT
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public string DecryptToString(string base64) => Encoding.UTF8.GetString(Decrypt(base64));
        public string DecryptToString(byte[] encryptedData) => Encoding.UTF8.GetString(Decrypt(encryptedData));
        public byte[] Decrypt(string base64) => Decrypt(Convert.FromBase64String(base64));
        public byte[] Decrypt(byte[] encryptedData)
        {
            if (secretKey == null) throw new KeyNotFoundException();

            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(secretKey, iv), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedData, 0, encryptedData.Length);
                }
                xBuff = ms.ToArray();
            }
            return xBuff;
        }

    }
}