using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public unsafe class MemoryPoolTest : TimeChecker
    {
        private const int CNT = 100_000_000;

        public MemoryPoolTest()
        {
            MemoryPool pool = new MemoryPool(32 * 1024 * 1024);

            while (true)
            {
                BeginSample("Struct in stack");
                for (int i = 0; i < CNT; i++)
                {
                    ABC abc = new ABC();
                    abc.a = i;
                    abc.b = i + 1;
                    abc.c = i + 2;
                }
                for (int i = 0; i < CNT; i++)
                {
                    ABC2 abc = new ABC2();
                    abc.a = (byte)i;
                    abc.b = i + 1;
                    abc.c = (byte)(i + 2);
                }
                EndSample();

                BeginSample("Class");
                for (int i = 0; i < CNT; i++)
                {
                    cABC abc = new cABC();
                    abc.a = i;
                    abc.b = i + 1;
                    abc.c = i + 2;
                }
                for (int i = 0; i < CNT; i++)
                {
                    cABC2 abc = new cABC2();
                    abc.a = (byte)i;
                    abc.b = i + 1;
                    abc.c = (byte)(i + 2);
                }
                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                EndSample();

                BeginSample("MemoryPool");
                for (int i = 0; i < CNT; i++)
                {
                    ABC* abc = (ABC*)pool.Alloc(sizeof(ABC));
                    abc->a = i;
                    abc->b = i + 1;
                    abc->c = i + 2;
                    pool.Free(abc);
                }
                for (int i = 0; i < CNT; i++)
                {
                    ABC2* abc = (ABC2*)pool.Alloc(sizeof(ABC2));
                    abc->a = (byte)i;
                    abc->b = i + 1;
                    abc->c = (byte)(i + 2);
                    pool.Free(abc);
                }
                pool.Clear();
                EndSample();

                BeginSample("Marshal (cnt/10)");
                for (int i = 0; i < CNT / 10; i++)
                {
                    ABC* abc = (ABC*)Marshal.AllocHGlobal(sizeof(ABC));
                    abc->a = i;
                    abc->b = i + 1;
                    abc->c = i + 2;
                    Marshal.FreeHGlobal(new IntPtr(abc));
                }
                for (int i = 0; i < CNT / 10; i++)
                {
                    ABC2* abc = (ABC2*)Marshal.AllocHGlobal(sizeof(ABC2));
                    abc->a = (byte)i;
                    abc->b = i + 1;
                    abc->c = (byte)(i + 2);
                    Marshal.FreeHGlobal(new IntPtr(abc));
                }
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();

                PrintSamples();
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ABC
        {
            public int a;
            public int b;
            public int c;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ABC2
        {
            public byte a;
            public int b;
            public byte c;
        }

        public class cABC
        {
            public int a;
            public int b;
            public int c;
        }

        public class cABC2
        {
            public byte a;
            public int b;
            public byte c;
        }

        public unsafe class MemoryPool : IDisposable
        {
            private const int SIZE = 0x7FFFFFFF;
            private const uint FLAG = 0x80000000;
            private const int HSIZE = sizeof(int);

            private byte* m_ptrStart;
            private byte* m_ptrEnd;
            private byte* m_ptrLast;
            private int m_Length;

            public MemoryPool(int size)
            {
                m_ptrLast = m_ptrStart = (byte*)Marshal.AllocHGlobal(size);
                m_ptrEnd = m_ptrStart + size;
                m_Length = size;
            }

            ~MemoryPool()
            {
                Dispose();
            }

            public void Dispose()
            {
                if (m_ptrStart == (void*)0)
                {
                    return;
                }

                Marshal.FreeHGlobal(new IntPtr(m_ptrStart));
                m_ptrStart = m_ptrEnd = m_ptrLast = (byte*)0;
            }

            public void Clear()
            {
                var span = new Span<byte>(m_ptrStart, m_Length);
                span.Fill(0);
            }

            public byte* Alloc(int size)
            {
                int header, hsize;

            Research:
                //---------------------------------------------------------------------------------
                // 남은 공간이 size로 할당하기에 충분한 공간인지 검사한다.
                if (m_ptrLast + size + HSIZE >= m_ptrEnd)
                {
                    // 초기 위치로 되돌린다.
                    m_ptrLast = m_ptrStart;

                    // 초기 위치로부터도 할당할 수 없다면 오류를 내보낸다.
                    if (m_ptrLast + size + 1 > m_ptrEnd)
                    {
                        throw new OutOfMemoryException();
                    }

                    // 비어있는 공간을 찾는다.
                    for (; m_ptrLast + size + HSIZE < m_ptrEnd; m_ptrLast += hsize + HSIZE)
                    {
                        header = *((int*)m_ptrLast);
                        hsize = *((int*)m_ptrLast) & SIZE;
                        if ((header & FLAG) == 0 && size <= hsize)
                        {
                            goto Alloc;
                        }

                        m_ptrLast += hsize + HSIZE;
                    }

                    // 할당 가능한 공간이 없다.
                    throw new OutOfMemoryException();
                }
                else
                {
                    //---------------------------------------------------------------------------------
                    // 다음 할당 지점을 검색한다.
                    for (; m_ptrLast + size + HSIZE < m_ptrEnd; m_ptrLast += hsize + HSIZE)
                    {
                        header = *((int*)m_ptrLast);
                        hsize = *((int*)m_ptrLast) & SIZE;

                        // 사용중이지 않으면서, 할당할 사이즈보다 적다면 할당을 진행한다.
                        if ((header & FLAG) == 0 && (hsize == 0 || size <= hsize))
                        {
                            goto Alloc;
                        }
                    }

                    // 할당에 실패했다면 초기할당검사로 이동한다.
                    goto Research;
                }

            Alloc:
                //---------------------------------------------------------------------------------
                // 할당을 진행한다.

                // 이전에 할당된 블럭이 아니라면 블럭에 사이즈를 추가한다.
                if (hsize == 0)
                {
                    *((int*)m_ptrLast) = unchecked((int)((size & SIZE) | FLAG));
                }
                // 블럭에 사이즈가 추가된 상태라면 flag만 추가한다.
                else
                {
                    *((int*)m_ptrLast) |= unchecked((int)FLAG);
                }

                // 할당
                byte* rst = m_ptrLast + 4;

                // 다음 위치 지정
                m_ptrLast = rst + size;

                return rst;
            }

            public void Free(void* ptr)
            {
                *((int*)ptr - 1) &= SIZE;
            }
        }
    }
}
