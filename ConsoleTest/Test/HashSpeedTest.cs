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
    public class HashSpeedTest
    {
        public HashSpeedTest()
        {
            long loop = 100000;

            string[] a = new[]
            {
                "a",
                "return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));",
                "return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));",
            };

            string b = "";

            var p = new TimeChecker();
            for (int i = 0; i < a.Length; i++)
            {
                p.BeginSample("Md5 " + i);
                for (int j = 0; j < loop; j++)
                {
                    b = a[i].Md5();
                }
                p.EndSample();
                p.BeginSample("Sha1 " + i);
                for (int j = 0; j < loop; j++)
                {
                    b = a[i].Sha1();
                }
                p.EndSample();
                p.BeginSample("Sha256 " + i);
                for (int j = 0; j < loop; j++)
                {
                    b = a[i].Sha256();
                }
                p.EndSample();
                p.BeginSample("Aes256 " + i);
                for (int j = 0; j < loop; j++)
                {
                    b = a[i].Aes256();
                }
                p.EndSample();
            }

            p.PrintSamples();
        }

    }
    public static class Enc
    {
        public static string Md5(this string msg)
        {
            using (MD5 enc = MD5.Create())
            {
                return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));
            }
        }

        public static string Sha1(this string msg)
        {
            using (SHA1 enc = SHA1.Create())
            {
                return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));
            }
        }

        public static string Sha256(this string msg)
        {
            using (SHA256 enc = SHA256.Create())
            {
                return Hex2Str(enc.ComputeHash(Encoding.UTF8.GetBytes(msg)));
            }
        }

        private static Aes256 aes256;
        public static string Aes256(this string msg)
        {
            if (aes256 == null)
            {
                aes256 = new Aes256(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            }
            return aes256.EncryptToBase64(msg);
        }

        private static string[] hexs = new string[256];
        static Enc()
        {
            hexs = new string[256];
            for (int a = 0; a <= byte.MaxValue; a++)
            {
                hexs[a] = a.ToString("X2");
            }
        }
        public static string Hex2Str(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            foreach (var b in data)
            {
                sb.Append(hexs[b]);
            }
            return sb.ToString();
        }
    }

}