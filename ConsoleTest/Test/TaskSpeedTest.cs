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
    public class TaskSpeedTest : TimeChecker
    {
        public TaskSpeedTest()
        {
            RunTask().Wait();
        }

        int add;
        private async Task RunTask()
        {
            while (true)
            {
                int cnt = 10000000;
                add = 0;

                BeginSample("Nope");
                for (int i = 0; i < cnt; i++)
                {
                    add++;
                }
                EndSample();

                BeginSample("Method");
                for (int i = 0; i < cnt; i++)
                {
                    Add();
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    void Add()
                    {
                        add++;
                    }
                }
                EndSample();

                BeginSample("Method Deep");
                for (int i = 0; i < cnt; i++)
                {
                    Add();
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
                EndSample();

                BeginSample("Method ScaleOut");
                for (int i = 0; i < cnt; i++)
                {
                    Add();
                    Add2();
                    Add3();
                    Add4();
                    Add5();
                    Add6();
                    Add7();
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    void Add()
                    {
                        add += 1;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    void Add2()
                    {
                        add += 2;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    void Add3()
                    {
                        add -= 1;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    void Add4()
                    {
                        add += 4;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    void Add5()
                    {
                        add -= 5;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    void Add6()
                    {
                        add += 6;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    void Add7()
                    {
                        add++;
                    }
                }
                EndSample();

                BeginSample("await Complete");
                for (int i = 0; i < cnt; i++)
                {
                    await Add();
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    Task Add()
                    {
                        add++;
                        return Task.CompletedTask;
                    }
                }
                EndSample();

                BeginSample("await Complete Deep");
                for (int i = 0; i < cnt; i++)
                {
                    await Add();
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
                EndSample();

                BeginSample("await Complete Scaleout");
                for (int i = 0; i < cnt; i++)
                {
                    await Add();
                    await Add2();
                    await Add3();
                    await Add4();
                    await Add5();
                    await Add6();
                    await Add7();
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    async Task Add()
                    {
                        add += 1;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    async Task Add2()
                    {
                        add += 2;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    async Task Add3()
                    {
                        add -= 1;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    async Task Add4()
                    {
                        add += 4;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    async Task Add5()
                    {
                        add -= 5;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    async Task Add6()
                    {
                        add += 6;
                    }
                    [MethodImpl(MethodImplOptions.NoInlining)]
                    async Task Add7()
                    {
                        add++;
                    }
                }
                EndSample();

                //BeginSample("await Yield");
                //for (int i = 0; i < cnt; i++)
                //{
                //    await Task.Yield();
                //    add++;
                //}
                //EndSample();

                PrintSamples();
            }
        }
    }
}
