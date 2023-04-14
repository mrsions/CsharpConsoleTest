using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class ToHexSpeedTest : TimeChecker
    {

        static byte[] HEXTABLE1;

        public static byte RESULT;

        public ToHexSpeedTest()
        {
            HEXTABLE1 = new byte['f' + 1];

            for (int i = 0, iLen = HEXTABLE1.Length; i < iLen; i++) HEXTABLE1[i] = (byte)255;
            for (int i = 0; i < 6; i++)
            {
                HEXTABLE1['a' + i] = (byte)(i + 10);
                HEXTABLE1['A' + i] = (byte)(i + 10);
            }
            for (int i = 0; i < 10; i++)
            {
                HEXTABLE1['0' + i] = (byte)(i);
            }

            int cnt = 10000000;

            while (true)
            {
                Proc("Test0", cnt, () =>
                {
                    byte result = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        result = ToHex((char)(i + 'a'));
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        result = ToHex((char)(i + 'A'));
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        result = ToHex((char)(i + '0'));
                    }
                    RESULT = result;
                });
                Proc("Test1", cnt, () =>
                {
                    byte result = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        result = ToHex1((char)(i + 'a'));
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        result = ToHex1((char)(i + 'A'));
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        result = ToHex1((char)(i + '0'));
                    }
                    RESULT = result;
                });
                Proc("Test2", cnt, () =>
                {
                    byte result = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        result = ToHex2((char)(i + 'a'));
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        result = ToHex2((char)(i + 'A'));
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        result = ToHex2((char)(i + '0'));
                    }
                    RESULT = result;
                });
                Proc("Test3", cnt, () =>
                {
                    byte result = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        result = ToHex3((char)(i + 'a'));
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        result = ToHex3((char)(i + 'A'));
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        result = ToHex3((char)(i + '0'));
                    }
                    RESULT = result;
                });


                Proc("Inline - Test0", cnt, () =>
                {
                    byte result = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        result = InlineToHex((char)(i + 'a'));
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        result = InlineToHex((char)(i + 'A'));
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        result = InlineToHex((char)(i + '0'));
                    }
                    RESULT = result;
                });
                Proc("Inline - Test1", cnt, () =>
                {
                    byte result = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        result = InlineToHex1((char)(i + 'a'));
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        result = InlineToHex1((char)(i + 'A'));
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        result = InlineToHex1((char)(i + '0'));
                    }
                    RESULT = result;
                });
                Proc("Inline - Test2", cnt, () =>
                {
                    byte result = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        result = InlineToHex2((char)(i + 'a'));
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        result = InlineToHex2((char)(i + 'A'));
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        result = InlineToHex2((char)(i + '0'));
                    }
                    RESULT = result;
                });
                Proc("Inline - Test3", cnt, () =>
                {
                    byte result = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        result = InlineToHex3((char)(i + 'a'));
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        result = InlineToHex3((char)(i + 'A'));
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        result = InlineToHex3((char)(i + '0'));
                    }
                    RESULT = result;
                });

                PrintSamples();
            }
        }

        private byte ToHex(char v)
        {
            if ('0' <= v && v <= '9')
            {
                return (byte)(v - '0');
            }
            else if ('a' <= v && v <= 'f')
            {
                return (byte)(v - 'a' + 10);
            }
            else if ('A' <= v && v <= 'F')
            {
                return (byte)(v - 'A' + 10);
            }
            return 0;
        }

        private byte ToHex1(char v)
        {
            const int ADD1 = 'A' - '0' - 10;
            const int ADD2 = 'a' - 'A';

            int result = v - '0';
            if (result < 0) throw new InvalidCastException();
            else if (result < 10) return (byte)result;

            result -= ADD1;
            if (result < 0) throw new InvalidCastException();
            else if (result < 16) return (byte)result;

            result -= ADD2;
            if (result < 0) throw new InvalidCastException();
            else if (result < 16) return (byte)result;

            throw new InvalidCastException();
        }

        private byte ToHex2(char v)
        {
            byte result = HEXTABLE1[v];
            return result == 255 ? throw new InvalidCastException() : result;
        }

        private byte ToHex3(char v)
        {
            return HEXTABLE1[v] == 255 ? throw new InvalidCastException() : HEXTABLE1[v];
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte InlineToHex(char v)
        {
            if ('a' <= v && v <= 'f')
            {
                return (byte)(v - 'a' + 10);
            }
            else if ('A' <= v && v <= 'F')
            {
                return (byte)(v - 'A' + 10);
            }
            else if ('0' <= v && v <= '9')
            {
                return (byte)(v - '0');
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte InlineToHex1(char v)
        {
            const int ADD1 = 'A' - '0' - 10;
            const int ADD2 = 'a' - 'A';

            int result = v - '0';
            if (result < 0) throw new InvalidCastException();
            else if (result < 10) return (byte)result;

            result -= ADD1;
            if (result < 0) throw new InvalidCastException();
            else if (result < 16) return (byte)result;

            result -= ADD2;
            if (result < 0) throw new InvalidCastException();
            else if (result < 16) return (byte)result;

            throw new InvalidCastException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte InlineToHex2(char v)
        {
            byte result = HEXTABLE1[v];
            return result == 255 ? throw new InvalidCastException() : result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte InlineToHex3(char v)
        {
            return HEXTABLE1[v] == 255 ? throw new InvalidCastException() : HEXTABLE1[v];
        }
    }
}
