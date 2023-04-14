using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest
{
    public class Hex2String : TimeChecker
    {
        public Hex2String()
        {
            const string a = "asdflaskj3lk2ntl2mtpogdi nugp34r mtwe4;tlkwjkot325lrjgoidzsfjg";
            int len = 100000;
            byte[] aa = Encoding.UTF8.GetBytes(a);

            while (true)
            {
                ClearTime();

                Proc("A", len, () =>
                {
                    Hex(aa);
                });
                Proc("B", len, () =>
                {
                    Hex2(aa);
                });
                Proc("C", len, () =>
                {
                    Hex3(aa);
                });
                Proc("D", len, () =>
                {
                    Hex4(aa);
                });

                PrintSamples();
            }
        }

        public static string Hex(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        public static string Hex2(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            foreach (var b in data)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        private static string[] hexs = new string[256];
        static Hex2String()
        {
            hexs = new string[256];
            for(int a=0; a<=byte.MaxValue; a++)
            {
                hexs[a] = a.ToString("X2");
            }
        }
        public static string Hex3(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            foreach (var b in data)
            {
                sb.Append(hexs[b]);
            }
            return sb.ToString();
        }
        public static string Hex4(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            foreach (var b in data)
            {
                switch ((b >> 4) & 0xF)
                {
                    case 0x0: sb.Append("0"); break;
                    case 0x1: sb.Append("1"); break;
                    case 0x2: sb.Append("2"); break;
                    case 0x3: sb.Append("3"); break;
                    case 0x4: sb.Append("4"); break;
                    case 0x5: sb.Append("5"); break;
                    case 0x6: sb.Append("6"); break;
                    case 0x7: sb.Append("7"); break;
                    case 0x8: sb.Append("8"); break;
                    case 0x9: sb.Append("9"); break;
                    case 0xA: sb.Append("A"); break;
                    case 0xB: sb.Append("B"); break;
                    case 0xC: sb.Append("C"); break;
                    case 0xD: sb.Append("D"); break;
                    case 0xE: sb.Append("E"); break;
                    case 0xF: sb.Append("F"); break;
                }
                switch (b & 0xF)
                {
                    case 0x0: sb.Append("0"); break;
                    case 0x1: sb.Append("1"); break;
                    case 0x2: sb.Append("2"); break;
                    case 0x3: sb.Append("3"); break;
                    case 0x4: sb.Append("4"); break;
                    case 0x5: sb.Append("5"); break;
                    case 0x6: sb.Append("6"); break;
                    case 0x7: sb.Append("7"); break;
                    case 0x8: sb.Append("8"); break;
                    case 0x9: sb.Append("9"); break;
                    case 0xA: sb.Append("A"); break;
                    case 0xB: sb.Append("B"); break;
                    case 0xC: sb.Append("C"); break;
                    case 0xD: sb.Append("D"); break;
                    case 0xE: sb.Append("E"); break;
                    case 0xF: sb.Append("F"); break;
                }
            }
            return sb.ToString();
        }
    }
}
