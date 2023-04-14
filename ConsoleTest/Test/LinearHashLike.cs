using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest.Test
{
    public class LinearHashLike
    {
        //public static byte[] Encode(byte[] key, byte[] data)
        //{

        //}

        public class IndexCipher2
        {
            const int MAX = byte.MaxValue + 1;
            const int SEED = 192234561;

            readonly static byte[] s_EncryptionSource = new byte[MAX];
            readonly static byte[] s_DecryptionSource = new byte[MAX];

            /// <summary>
            /// 초기화
            /// </summary>
            static IndexCipher2()
            {
                var enc = s_EncryptionSource = new byte[MAX];
                var dec = s_DecryptionSource = new byte[MAX];

                for (int i = 0; i < MAX; i++)
                {
                    s_EncryptionSource[i] = (byte)i;
                }

                Random rnd = new Random(SEED);
                for (int i = 0; i < MAX; i++)
                {
                    int ti = rnd.Next(0, MAX);
                    (enc[i], enc[ti]) = (enc[ti], enc[i]);
                }

                for (int i = 0; i < MAX; i++)
                {
                    dec[enc[i]] = (byte)i;
                }
            }

            public void Encryption(byte[] data, int offset, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    data[offset + i] = s_EncryptionSource[data[offset + i]];
                }

                for (int i = 1; i < length; i++)
                {
                    data[offset + i] = (byte)((data[offset + i] + s_EncryptionSource[data[offset + i - 1]]) & 0xFF);
                }
            }

            public void Decryption(byte[] data, int offset, int length)
            {
                for (int i = length - 1; i > 0; i--)
                {
                    data[offset + i] = (byte)((data[offset + i] - s_EncryptionSource[data[offset + i - 1]]) & 0xFF);
                }

                for (int i = 0; i < length; i++)
                {
                    data[offset + i] = s_DecryptionSource[data[offset + i]];
                }
            }

            public void Encryption(Span<byte> data)
            {
                int length = data.Length;
                for (int i = 0; i < length; i++)
                {
                    data[i] = s_EncryptionSource[data[i]];
                }

                for (int i = 1; i < length; i++)
                {
                    data[i] = (byte)((data[i] + s_EncryptionSource[data[i - 1]]) & 0xFF);
                }
            }

            public void Decryption(Span<byte> data)
            {
                int length = data.Length;
                for (int i = length - 1; i > 0; i--)
                {
                    data[i] = (byte)((data[i] - s_EncryptionSource[data[i - 1]]) & 0xFF);
                }

                for (int i = 0; i < length; i++)
                {
                    data[i] = s_DecryptionSource[data[i]];
                }
            }
        }
    }
}
