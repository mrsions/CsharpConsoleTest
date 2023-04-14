using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest.Test
{
    public class IndexCipherTest : TimeChecker
    {
        public IndexCipherTest()
        {
            byte[] data = new byte[10000];
            new Random().NextBytes(data);
            int loop = 100000;

            while (true)
            {
                byte[] test = null;

                BeginSample("TestA");
                for (int i = 0; i < loop; i++)
                {
                    test = data.CloneArray();
                }
                EndSample();

                BeginSample("TestB");
                for (int i = 0; i < loop; i++)
                {
                    test = (byte[])data.Clone();
                }
                EndSample();

                BeginSample("TestC");
                for (int i = 0; i < loop; i++)
                {
                    test = new byte[data.Length];
                    for (int j = 0; j < test.Length; j++)
                    {
                        test[j] = data[j];
                    }
                }
                EndSample();

                BeginSample("TestD");
                for (int i = 0; i < loop; i++)
                {
                    if (!data.EqualsArray(test))
                    {
                        Console.WriteLine("B");
                    }
                }
                EndSample();

                BeginSample("TestE");
                for (int i = 0; i < loop; i++)
                {
                    if (!EqualsArray(data, test))
                    {
                        Console.WriteLine("B");
                    }
                }
                EndSample();

                // 10배 이상 느림
                //BeginSample("TestF");
                //for (int i = 0; i < loop; i++)
                //{
                //    if (!data.SequenceEqual(test))
                //    {
                //        Console.WriteLine("B");
                //    }
                //}
                //EndSample();

                PrintSamples();
            }
        }

        bool EqualsArray(byte[] array, byte[] target)
        {
            for (int i = 0, iLen = array.Length; i < iLen; i++)
            {
                if (!array[i].Equals(target[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public class IndexCipher
        {
            static byte[] EncryptionSource = new byte[]
            {
            (byte) 251, (byte) 26, (byte) 77, (byte) 216, (byte) 169, (byte) 67, (byte) 55, (byte) 190, (byte) 219, (byte) 9, (byte) 149, (byte) 156, (byte) 76, (byte) 112, (byte) 144, (byte) 137, (byte) 200, (byte) 161, (byte) 126, (byte) 5, (byte) 80, (byte) 66, (byte) 201, (byte) 72, (byte) 110, (byte) 177, (byte) 114, (byte) 2, (byte) 192, (byte) 41, (byte) 16, (byte) 131, (byte) 226, (byte) 21, (byte) 13, (byte) 125, (byte) 166, (byte) 243, (byte) 123, (byte) 154, (byte) 147, (byte) 159, (byte) 255, (byte) 168, (byte) 109, (byte) 78, (byte) 176, (byte) 71, (byte) 175, (byte) 132, (byte) 45, (byte) 11, (byte) 93, (byte) 81, (byte) 105, (byte) 185, (byte) 254, (byte) 164, (byte) 210, (byte) 172, (byte) 250, (byte) 8, (byte) 116, (byte) 23, (byte) 31, (byte) 253, (byte) 165, (byte) 212, (byte) 145, (byte) 187, (byte) 150, (byte) 134, (byte) 22, (byte) 75, (byte) 135, (byte) 58, (byte) 213, (byte) 171, (byte) 7, (byte) 229, (byte) 115, (byte) 244, (byte) 107, (byte) 214, (byte) 208, (byte) 173, (byte) 46, (byte) 170, (byte) 91, (byte) 139, (byte) 207, (byte) 42, (byte) 15, (byte) 238, (byte) 28, (byte) 129, (byte) 188, (byte) 88, (byte) 128, (byte) 36, (byte) 124, (byte) 118, (byte) 204, (byte) 206, (byte) 89, (byte) 142, (byte) 178, (byte) 245, (byte) 199, (byte) 153, (byte) 64, (byte) 63, (byte) 196, (byte) 24, (byte) 179, (byte) 18, (byte) 6, (byte) 4, (byte) 120, (byte) 195, (byte) 252, (byte) 74, (byte) 83, (byte) 119, (byte) 237, (byte) 70, (byte) 218, (byte) 27, (byte) 140, (byte) 43, (byte) 30, (byte) 223, (byte) 37, (byte) 48, (byte) 227, (byte) 235, (byte) 98, (byte) 194, (byte) 221, (byte) 1, (byte) 65, (byte) 111, (byte) 184, (byte) 40, (byte) 246, (byte) 86, (byte) 230, (byte) 231, (byte) 232, (byte) 160, (byte) 3, (byte) 14, (byte) 53, (byte) 220, (byte) 222, (byte) 0, (byte) 205, (byte) 248, (byte) 186, (byte) 247, (byte) 224, (byte) 32, (byte) 84, (byte) 54, (byte) 197, (byte) 152, (byte) 163, (byte) 225, (byte) 50, (byte) 138, (byte) 33, (byte) 127, (byte) 182, (byte) 117, (byte) 100, (byte) 133, (byte) 234, (byte) 148, (byte) 202, (byte) 157, (byte) 60, (byte) 12, (byte) 104, (byte) 61, (byte) 146, (byte) 52, (byte) 20, (byte) 99, (byte) 151, (byte) 103, (byte) 203, (byte) 57, (byte) 236, (byte) 25, (byte) 95, (byte) 122, (byte) 68, (byte) 249, (byte) 189, (byte) 44, (byte) 167, (byte) 73, (byte) 108, (byte) 59, (byte) 38, (byte) 96, (byte) 181, (byte) 106, (byte) 113, (byte) 79, (byte) 228, (byte) 85, (byte) 209, (byte) 35, (byte) 174, (byte) 215, (byte) 102, (byte) 198, (byte) 136, (byte) 29, (byte) 121, (byte) 39, (byte) 17, (byte) 49, (byte) 193, (byte) 155, (byte) 191, (byte) 19, (byte) 82, (byte) 62, (byte) 242, (byte) 217, (byte) 158, (byte) 97, (byte) 10, (byte) 90, (byte) 87, (byte) 239, (byte) 51, (byte) 94, (byte) 180, (byte) 211, (byte) 130, (byte) 233, (byte) 47, (byte) 69, (byte) 241, (byte) 56, (byte) 183, (byte) 141, (byte) 101, (byte) 240, (byte) 162, (byte) 34, (byte) 92, (byte) 143
            };

            static byte[] DecrypionSource = new byte[]
            {
            (byte) 155, (byte) 139, (byte) 27, (byte) 150, (byte) 117, (byte) 19, (byte) 116, (byte) 78, (byte) 61, (byte) 9, (byte) 234, (byte) 51, (byte) 181, (byte) 34, (byte) 151, (byte) 92, (byte) 30, (byte) 222, (byte) 115, (byte) 227, (byte) 186, (byte) 33, (byte) 72, (byte) 63, (byte) 113, (byte) 193, (byte) 1, (byte) 127, (byte) 94, (byte) 219, (byte) 130, (byte) 64, (byte) 161, (byte) 170, (byte) 253, (byte) 213, (byte) 99, (byte) 132, (byte) 204, (byte) 221, (byte) 143, (byte) 29, (byte) 91, (byte) 129, (byte) 199, (byte) 50, (byte) 86, (byte) 244, (byte) 133, (byte) 223, (byte) 168, (byte) 238, (byte) 185, (byte) 152, (byte) 163, (byte) 6, (byte) 247, (byte) 191, (byte) 75, (byte) 203, (byte) 180, (byte) 183, (byte) 229, (byte) 111, (byte) 110, (byte) 140, (byte) 21, (byte) 5, (byte) 196, (byte) 245, (byte) 125, (byte) 47, (byte) 23, (byte) 201, (byte) 121, (byte) 73, (byte) 12, (byte) 2, (byte) 45, (byte) 209, (byte) 20, (byte) 53, (byte) 228, (byte) 122, (byte) 162, (byte) 211, (byte) 145, (byte) 236, (byte) 97, (byte) 104, (byte) 235, (byte) 88, (byte) 254, (byte) 52, (byte) 239, (byte) 194, (byte) 205, (byte) 233, (byte) 136, (byte) 187, (byte) 174, (byte) 250, (byte) 216, (byte) 189, (byte) 182, (byte) 54, (byte) 207, (byte) 82, (byte) 202, (byte) 44, (byte) 24, (byte) 141, (byte) 13, (byte) 208, (byte) 26, (byte) 80, (byte) 62, (byte) 173, (byte) 101, (byte) 123, (byte) 118, (byte) 220, (byte) 195, (byte) 38, (byte) 100, (byte) 35, (byte) 18, (byte) 171, (byte) 98, (byte) 95, (byte) 242, (byte) 31, (byte) 49, (byte) 175, (byte) 71, (byte) 74, (byte) 218, (byte) 15, (byte) 169, (byte) 89, (byte) 128, (byte) 249, (byte) 105, (byte) 255, (byte) 14, (byte) 68, (byte) 184, (byte) 40, (byte) 177, (byte) 10, (byte) 70, (byte) 188, (byte) 165, (byte) 109, (byte) 39, (byte) 225, (byte) 11, (byte) 179, (byte) 232, (byte) 41, (byte) 149, (byte) 17, (byte) 252, (byte) 166, (byte) 57, (byte) 66, (byte) 36, (byte) 200, (byte) 43, (byte) 4, (byte) 87, (byte) 77, (byte) 59, (byte) 85, (byte) 214, (byte) 48, (byte) 46, (byte) 25, (byte) 106, (byte) 114, (byte) 240, (byte) 206, (byte) 172, (byte) 248, (byte) 142, (byte) 55, (byte) 158, (byte) 69, (byte) 96, (byte) 198, (byte) 7, (byte) 226, (byte) 28, (byte) 224, (byte) 137, (byte) 119, (byte) 112, (byte) 164, (byte) 217, (byte) 108, (byte) 16, (byte) 22, (byte) 178, (byte) 190, (byte) 102, (byte) 156, (byte) 103, (byte) 90, (byte) 84, (byte) 212, (byte) 58, (byte) 241, (byte) 67, (byte) 76, (byte) 83, (byte) 215, (byte) 3, (byte) 231, (byte) 126, (byte) 8, (byte) 153, (byte) 138, (byte) 154, (byte) 131, (byte) 160, (byte) 167, (byte) 32, (byte) 134, (byte) 210, (byte) 79, (byte) 146, (byte) 147, (byte) 148, (byte) 243, (byte) 176, (byte) 135, (byte) 192, (byte) 124, (byte) 93, (byte) 237, (byte) 251, (byte) 246, (byte) 230, (byte) 37, (byte) 81, (byte) 107, (byte) 144, (byte) 159, (byte) 157, (byte) 197, (byte) 60, (byte) 0, (byte) 120, (byte) 65, (byte) 56, (byte) 42
            };

            public static byte[] Encryption(byte[] data, int offset, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    data[i] = EncryptionSource[data[offset + i]];
                }
                return data;
            }

            public static byte[] Decryption(byte[] data, int offset, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    data[i] = DecrypionSource[data[offset + i]];
                }
                return data;
            }
        }
    }
}
