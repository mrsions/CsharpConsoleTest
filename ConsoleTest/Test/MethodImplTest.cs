using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class MethodImplTest : TimeChecker
    {
        public MethodImplTest()
        {
            Task[] tasks = new Task[100];
            int cnt = 1000000;

            while (true)
            {
                object locko = new object();
                BeginSample("Native");
                for (int j = 0; j < tasks.Length; j++)
                {
                    tasks[j] = Task.Run(() =>
                    {
                        ABC abc = new ABC();
                        for (int i = 0; i < cnt; i++)
                        {
                            abc.a += i + (j - abc.a) * 2;
                        }
                    });
                }
                Task.WaitAll(tasks);
                EndSample();
                BeginSample("Do");
                for (int j = 0; j < tasks.Length; j++)
                {
                    tasks[j] = Task.Run(() =>
                    {
                        ABC abc = new ABC();
                        for (int i = 0; i < cnt; i++)
                        {
                            abc.Do(locko, abc, i, j);
                        }
                    });
                }
                Task.WaitAll(tasks);
                EndSample();
                BeginSample("DoSync");
                for (int j = 0; j < tasks.Length; j++)
                {
                    tasks[j] = Task.Run(() =>
                    {
                        ABC abc = new ABC();
                        for (int i = 0; i < cnt; i++)
                        {
                            abc.DoSync(locko, abc, i, j);
                        }
                    });
                }
                Task.WaitAll(tasks);
                EndSample();
                BeginSample("DoInline");
                for (int j = 0; j < tasks.Length; j++)
                {
                    tasks[j] = Task.Run(() =>
                    {
                        ABC abc = new ABC();
                        for (int i = 0; i < cnt; i++)
                        {
                            abc.DoInline(locko, abc, i, j);
                        }
                    });
                }
                Task.WaitAll(tasks);
                EndSample();
                BeginSample("DoSyncInline");
                for (int j = 0; j < tasks.Length; j++)
                {
                    tasks[j] = Task.Run(() =>
                    {
                        ABC abc = new ABC();
                        for (int i = 0; i < cnt; i++)
                        {
                            abc.DoSyncInline(locko, abc, i, j);
                        }
                    });
                }
                Task.WaitAll(tasks);
                EndSample();

                //BeginSample("Native (Single)");
                //for (int j = 0; j < tasks.Length; j++)
                //{

                //    {
                //        ABC abc = new ABC();
                //        for (int i = 0; i < cnt; i++)
                //        {
                //            abc.a += i + (j - abc.a) * 2;
                //        }
                //    }
                //}

                //EndSample();
                //BeginSample("Do (Single)");
                //for (int j = 0; j < tasks.Length; j++)
                //{

                //    {
                //        ABC abc = new ABC();
                //        for (int i = 0; i < cnt; i++)
                //        {
                //            abc.Do(i, j);
                //        }
                //    }
                //}
                //EndSample();
                //BeginSample("DoSync (Single)");
                //for (int j = 0; j < tasks.Length; j++)
                //{

                //    {
                //        ABC abc = new ABC();
                //        for (int i = 0; i < cnt; i++)
                //        {
                //            abc.DoSync(i, j);
                //        }
                //    }
                //}
                //EndSample();
                //BeginSample("DoInline (Single)");
                //for (int j = 0; j < tasks.Length; j++)
                //{

                //    {
                //        ABC abc = new ABC();
                //        for (int i = 0; i < cnt; i++)
                //        {
                //            abc.DoInline(i, j);
                //        }
                //    }
                //}
                //EndSample();
                //BeginSample("DoSyncInline (Single)");
                //for (int j = 0; j < tasks.Length; j++)
                //{

                //    {
                //        ABC abc = new ABC();
                //        for (int i = 0; i < cnt; i++)
                //        {
                //            abc.DoSyncInline(i, j);
                //        }
                //    }
                //}
                //EndSample();

                PrintSamples();
            }
        }

        public class ABC
        {
            public int a;

            object L = new object();

            public void Do(object o, ABC abc, int i, int j)
            {
                abc.a += i + (j - abc.a) * 2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void DoSync(object o, ABC abc, int i, int j)
            {
                {
                    abc.a += i + (j - abc.a) * 2;
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void DoInline(object o, ABC abc, int i, int j)
            {
                abc.a += i + (j - abc.a) * 2;
            }
            //[MethodImpl(MethodImplOptions.AggressiveInlining)]
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.Synchronized)]
            public void DoSyncInline(object o, ABC abc, int i, int j)
            {
                {
                    abc.a += i + (j - abc.a) * 2;
                }
            }
        }
    }
}
