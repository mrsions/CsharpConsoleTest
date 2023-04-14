using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class BitBoolArrayTest : TimeChecker
    {
        public BitBoolArrayTest()
        {
            const int count = 1000;
            const int fcount = 100;
            var bb = new BitBoolArray(100000);
            BitArray2 bc = new BitArray2(100000);
            System.Collections.BitArray ba = new System.Collections.BitArray(100000);

            //for (int i = 0; i < bb.Count; i++)
            //{
            //    bb[i] = true;
            //    Debug.Assert(bb[i]);
            //}

            //bb.Fill(false);
            //foreach (var v in bb) Debug.Assert(!v);


            //bool b = true;
            //bool[] fills = new bool[bb.Count];
            //for (int j = 0; j < bb.Count / 1000; j++)
            //{
            //    for (int i = 1; i < bb.Count - (bb.Count / 1000); i++)
            //    {
            //        if (i + j > fills.Length) continue;
            //        bb.FillRange(b = !b, j, i);
            //        Array.Fill(fills, b, j, i);

            //        for (int k = 0; k < fills.Length; k++)
            //        {
            //            Debug.Assert(bb[k] == fills[k]);
            //        }
            //    }
            //}

            //bb.Fill(true);
            //foreach (var v in bb) Debug.Assert(v);

            while (true)
            {

                BeginSample("BitBoolArraySion");
                for (int k = 0, kLen = count; k < kLen; k++)
                {
                    for (int i = 0; i < bb.Count; i++)
                    {
                        bb[i] = !bb[i];
                    }
                };
                EndSample();

                BeginSample("BitArray");
                for (int k = 0, kLen = count; k < kLen; k++)
                {
                    for (int i = 0; i < ba.Count; i++)
                    {
                        ba[i] = !ba[i];
                    }
                };
                EndSample();

                BeginSample("BitArray2");
                for (int k = 0, kLen = count; k < kLen; k++)
                {
                    for (int i = 0; i < bc.Count; i++)
                    {
                        bc[i] = !bc[i];
                    }
                };
                EndSample();

                BeginSample("BitBoolArraySion-FillRange");
                for (int k = 0, kLen = count * fcount; k < kLen; k++)
                {
                    var v = bb.Count / 25;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            bb.FillRange(k % 2 == 0, v * i, v * l);
                        }
                    }
                };
                EndSample();

                BeginSample("BitBoolArraySion-FillRange this");
                for (int k = 0, kLen = count; k < kLen; k++)
                {
                    var v = bb.Count / 25;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            bool b = k % 2 == 0;
                            int ii = v * i;
                            int iiLen = v * l;
                            for (; ii < iiLen; ii++)
                            {
                                bb[k] = b;
                            }
                        }
                    }
                };
                EndSample();

                BeginSample("BitArray-FillRange this");
                for (int k = 0, kLen = count; k < kLen; k++)
                {
                    var v = ba.Count / 25;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            bool b = k % 2 == 0;
                            int ii = v * i;
                            int iiLen = v * l;
                            for (; ii < iiLen; ii++)
                            {
                                ba[k] = b;
                            }
                        }
                    }
                };
                EndSample();

                BeginSample("BitArray2-FillRange this");
                for (int k = 0, kLen = count; k < kLen; k++)
                {
                    var v = bc.Count / 25;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            bool b = k % 2 == 0;
                            int ii = v * i;
                            int iiLen = v * l;
                            for (; ii < iiLen; ii++)
                            {
                                bc[k] = b;
                            }
                        }
                    }
                };
                EndSample();

                BeginSample("BitBoolArraySion-Fill");
                for (int k = 0, kLen = count * fcount; k < kLen; k++)
                {
                    bb.Fill(k % 2 == 0);
                };
                EndSample();

                BeginSample("BitArray-Fill");
                for (int k = 0, kLen = count * fcount; k < kLen; k++)
                {
                    ba.SetAll(k % 2 == 0);
                };
                EndSample();

                BeginSample("BitArra2y-Fill");
                for (int k = 0, kLen = count * fcount; k < kLen; k++)
                {
                    bc.SetAll(k % 2 == 0);
                };
                EndSample();

                PrintSamples();
            }
        }




        public class BitArray2
        {
            private int[] m_array;

            private int m_length;

            public bool this[int index]
            {
                get
                {
                    return Get(index);
                }
                set
                {
                    Set(index, value);
                }
            }

            public int Count => m_length;

            public BitArray2(int length)
            {
                if (length < 0)
                {
                    throw new ArgumentOutOfRangeException("length", length, "ArgumentOutOfRange_NeedNonNegNum");
                }
                m_array = new int[GetInt32ArrayLengthFromBitLength(length)];
                m_length = length;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Get(int index)
            {
                if ((uint)index >= (uint)m_length)
                {
                    ThrowArgumentOutOfRangeException(index);
                }
                return (m_array[index >> 5] & (1 << index)) != 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Set(int index, bool value)
            {
                if ((uint)index >= (uint)m_length)
                {
                    ThrowArgumentOutOfRangeException(index);
                }
                int num = 1 << index;
                ref int reference = ref m_array[index >> 5];
                if (value)
                {
                    reference |= num;
                }
                else
                {
                    reference &= ~num;
                }
            }

            public void SetAll(bool value)
            {
                int int32ArrayLengthFromBitLength = GetInt32ArrayLengthFromBitLength(Count);
                Span<int> span = m_array.AsSpan(0, int32ArrayLengthFromBitLength);
                if (value)
                {
                    span.Fill(-1);
                    Div32Rem(m_length, out int remainder);
                    if (remainder > 0)
                    {
                        int index = span.Length - 1;
                        span[index] &= (1 << remainder) - 1;
                    }
                }
                else
                {
                    span.Clear();
                }
            }
            private static int GetInt32ArrayLengthFromBitLength(int n)
            {
                return (int)((uint)(n - 1 + 32) >> 5);
            }

            private static int GetInt32ArrayLengthFromByteLength(int n)
            {
                return (int)((uint)(n - 1 + 4) >> 2);
            }

            private static int GetByteArrayLengthFromBitLength(int n)
            {
                return (int)((uint)(n - 1 + 8) >> 3);
            }

            private static int Div32Rem(int number, out int remainder)
            {
                uint result = (uint)number / 32u;
                remainder = (number & 0x1F);
                return (int)result;
            }

            private static int Div4Rem(int number, out int remainder)
            {
                uint result = (uint)number / 4u;
                remainder = (number & 3);
                return (int)result;
            }

            private static void ThrowArgumentOutOfRangeException(int index)
            {
                throw new ArgumentOutOfRangeException("index", index, "ArgumentOutOfRange_Index");
            }
        }

    }
}
