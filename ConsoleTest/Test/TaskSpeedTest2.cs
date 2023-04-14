using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class TaskSpeedTest2 : TimeChecker
    {
        public TaskSpeedTest2()
        {
            if(!ThreadPool.SetMaxThreads(TaskCount * 2 + 2000, TaskCount * 2 + 2000) 
                | !ThreadPool.SetMinThreads(TaskCount * 2 + 1000, TaskCount * 2 + 1000))
            {

            }
            //useBeginLog = true;
            useClearPrint = false;
            while (true)
            {
                new RunThread(this, true);
                ForceGC();
                //new RunThread(this, false);
                //ForceGC();
                //new RunTask(this);
                //ForceGC();
                PrintSamples();
                Console.WriteLine();
                ForceGC();
            }
        }

        static int TaskCount = 10000;
        static int LoopCount = 10000;

        public class RunTask
        {
            bool start = false;
            int add = 0;
            int request = 0;
            int response = 0;

            public RunTask(TimeChecker tc)
            {
                tc.BeginSample("Task Make");
                for (int i = 0; i < TaskCount; i++)
                {
                    Running();
                }
                while (request != TaskCount)
                {
                    //Console.Write($"\r{request}/{response}");
                    Thread.Sleep(1);
                }
                tc.EndSample();
                tc.ForceGC();
                tc.BeginSample("Task Start");
                start = true;
                while (response != TaskCount)
                {
                    //Console.Write($"\r{request}/{response}");
                    Thread.Sleep(1);
                }
                tc.EndSample();
                Thread.Sleep(500);
                tc.ForceGC();
            }

            public async void Running()
            {
                lock (this)
                {
                    request++;
                }
                while (!start)
                {
                    await Task.Delay(1);
                }

                for (int i = 0; i < LoopCount; i++)
                {
                    await Add();
                }
                lock (this)
                {
                    response++;
                }
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            async Task Add()
            {
                add += 1;
                await Add2();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            async Task Add2()
            {
                add += 2;
                await Add3();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            async Task Add3()
            {
                add -= 1;
                await Add4();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            async Task Add4()
            {
                add += 4;
                await Add5();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            async Task Add5()
            {
                add -= 5;
                await Add6();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            async Task Add6()
            {
                await Add7();
                add += 6;
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            async Task Add7()
            {
                add++;
            }
        }

        public class RunThread
        {
            bool start = false;
            int add = 0;
            int request = 0;
            int response = 0;

            public RunThread(TimeChecker tc, bool usePool)
            {
                //ThreadPool.GetMinThreads(out var a, out var b);
                //ThreadPool.GetMaxThreads(out var c, out var d);
                //Console.WriteLine($"{ThreadPool.ThreadCount} / {ThreadPool.PendingWorkItemCount} / {ThreadPool.CompletedWorkItemCount} / {a}, {b} / {c}, {d}");

                tc.BeginSample($"Thread{(usePool ? "Pool" : "")} Make");
                if (usePool)
                {
                    for (int i = 0; i < TaskCount; i++)
                    {
                        //if (i % 100 == 0) Console.WriteLine(request);
                        ThreadPool.QueueUserWorkItem(delegate { Running(); });
                    }
                }
                else
                {
                    for (int i = 0; i < TaskCount; i++)
                    {
                        var t = new Thread(Running);
                        t.Start();
                    }
                }
                while (request != TaskCount)
                {
                    Thread.Sleep(1);
                }
                tc.EndSample();
                tc.ForceGC();
                Console.ReadLine();
                tc.BeginSample($"Thread{(usePool ? "Pool" : "")} Run");
                start = true;
                while (response != TaskCount)
                {
                    Thread.Sleep(1);
                }
                tc.EndSample();
                Thread.Sleep(500);
                tc.ForceGC();
            }

            public void Running()
            {
                lock (this)
                {
                    request++;
                }
                while (!start)
                {
                    Thread.Sleep(1);
                }

                for (int i = 0; i < LoopCount; i++)
                {
                    Add();
                }
                lock (this)
                {
                    response++;
                }
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            void Add()
            {
                add += 1;
                Add2();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            void Add2()
            {
                add += 2;
                Add3();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            void Add3()
            {
                add -= 1;
                Add4();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            void Add4()
            {
                add += 4;
                Add5();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            void Add5()
            {
                add -= 5;
                Add6();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            void Add6()
            {
                Add7();
                add += 6;
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            void Add7()
            {
                add++;
            }
        }
    }
}
