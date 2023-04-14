using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class StaticPerformanceTest : TimeChecker
    {
        public StaticPerformanceTest()
        {
            int cnt = 10000;
            while (true)
            {
                int rst = 0;
                BeginSample("Native");
                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < cnt; j++)
                    {
                        rst += i - ((i + j) * 2) / (j + 3) + i;
                    }
                }
                EndSample();
                BeginSample("Do");
                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < cnt; j++)
                    {
                        rst += ABC.Do(i, j);
                    }
                }
                EndSample();
                BeginSample("DoSync");
                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < cnt; j++)
                    {
                        rst += ABC.DoSync(i, j);
                    }
                }
                EndSample();
                BeginSample("DoSync StaticVar");
                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < cnt; j++)
                    {
                        rst += ABC.DoSync(i, j);
                    }
                }
                EndSample();
                BeginSample("DoInline");
                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < cnt; j++)
                    {
                        rst += ABC.DoInline(i, j);
                    }
                }
                EndSample();
                BeginSample("Native StaticVar");
                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < cnt; j++)
                    {
                        rst += ABC.a - ((ABC.a + j) * 2) / (j + 3)+ ABC.a;
                    }
                }
                EndSample();
                BeginSample("DoStatic1");
                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < cnt; j++)
                    {
                        rst += ABC.DoStatic1(i, j);
                    }
                }
                EndSample();
                BeginSample("DoStatic");
                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < cnt; j++)
                    {
                        rst += ABC.DoStatic(i, j);
                    }
                }
                EndSample();
                PrintSamples();
            }
        }

        public class ABC
        {
            public static int a = 1;
            static object LOCK = new object();

            public static int Do(int i, int j)
            {
                return i - ((i + j) * 2) / (j + 3) + i;
            }

            public static int DoStatic(int i, int j)
            {
                return a - ((a + j) * 2) / (j + 3) + a;
            }

            public static int DoStatic1(int i, int j)
            {
                return a - ((i + j) * 2) / (j + 3) + i;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static int DoSync(int i, int j)
            {
                return i - ((i + j) * 2) / (j + 3) + i;
            }

            public static int DoSyncStatic(int i, int j)
            {
                lock (LOCK)
                {
                    return i - ((i + j) * 2) / (j + 3) + i;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int DoInline(int i, int j)
            {
                return i - ((i + j) * 2) / (j + 3) + i;
            }
        }
    }
}
