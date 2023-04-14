using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using Realwith.NetPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleTest.Benchmark
{
    public class Bench_TBT_InstanceVsPool
    {
        static int[] ObjectSizes = new int[] { 24, 256  }; // milliseconds
        static int[] ObjectLifetimes = new int[] { 60_000, 360_000, 1080_000 }; // milliseconds
        const int IterationCount = 5;
        const int PoolSize = 1000;
        const int ThreadCount = 20;

        public static void Benchmark()
        {
            MultiThreadTester.Init(ThreadCount);

            using var fs = FileHelper.OpenWrite("d:/test.txt");
            using var writer = new StreamWriter(fs);
            Console.ReadLine();
            while (true)
            {
                foreach (var ol in ObjectLifetimes)
                {
                    Console.WriteLine("Lifetime " + ol);

                    foreach (var os in ObjectSizes)
                    {
                        Console.WriteLine("Size " + os);
                        GCUtils.FullGC();
                        Tester t = new Tester(os);

                        //warm up
                        t.A();
                        t.B();
                        t.C();
                        t.D();

                        // walk
                        t.lifetime = ol;
                        GCUtils.FullGC();
                        var b = t.B();
                        GCUtils.FullGC();
                        var c = t.C();
                        GCUtils.FullGC();
                        var a = t.A();
                        GCUtils.FullGC();
                        var d = t.D();

                        var msg = $"{ol,5} {os,5} {a,20:N0} {b,20:N0} {c,20:N0} {d,20:N0}";
                        Console.WriteLine(msg);
                        writer.WriteLine(msg);
                        writer.Flush();
                    }
                }
            }
        }

        public class Tester
        {
            private volatile bool isRun = true;

            private ThreadLocal<ObjectPool<byte[]>> pools;
            private LockedObjectPool<byte[]> lpool;
            private Action funcA, funcB, funcC, funcD;
            private object LOCK = new();
            private Func<byte[]> newFunc;
            private long totalCount;

            public int lifetime = 10;

            public Tester(int size)
            {
                newFunc = () => new byte[size];
                pools = new(() => new(PoolSize, newFunc));
                lpool = new(PoolSize, newFunc);
                funcA = delegate
                {
                    long cnt = 0;
                    while (isRun)
                    {
                        var obj = new byte[size];
                        for (int l = 0; l < obj.Length; l++)
                        {
                            obj[l] = (byte)(totalCount + l);
                        }
                        cnt++;
                    }
                    lock (this)
                    {
                        totalCount += cnt;
                    }
                };
                funcB = delegate
                {
                    var pool = lpool;
                    long cnt = 0;
                    while (isRun)
                    {
                        var obj = pool.Rent();
                        for (int l = 0; l < obj.Length; l++)
                        {
                            obj[l] = (byte)(totalCount + l);
                        }
                        lpool.Return(obj);
                        cnt++;
                    }
                    lock (this)
                    {
                        totalCount += cnt;
                    }
                };
                funcC = delegate
                {
                    long cnt = 0;
                    while (isRun)
                    {
                        var pool = pools.Value;
                        var obj = pool.Rent();
                        for (int l = 0; l < obj.Length; l++)
                        {
                            obj[l] = (byte)(totalCount + l);
                        }
                        pool.Return(obj);
                        cnt++;
                    }
                    lock (this)
                    {
                        totalCount += cnt;
                    }
                };
                funcD = delegate
                {
                    var pool = pools.Value;
                    long cnt = 0;
                    while (isRun)
                    {
                        var obj = pool.Rent();
                        for (int l = 0; l < obj.Length; l++)
                        {
                            obj[l] = (byte)(totalCount + l);
                        }
                        pool.Return(obj);
                        cnt++;
                    }
                    lock (this)
                    {
                        totalCount += cnt;
                    }
                };
            }

            public long A()
            {
                totalCount = 0;
                return Run(funcA);
            }

            public long B()
            {
                totalCount = 0;
                return Run(funcB);
            }

            public long C()
            {
                totalCount = 0;
                return Run(funcC);
            }

            public long D()
            {
                totalCount = 0;
                return Run(funcD);
            }

            private long Run(Action func)
            {
                for (int i = 0; i < IterationCount; i++)
                {
                    totalCount = 0;
                    isRun = true;
                    MultiThreadTester.Start(func);
                    Thread.Sleep(lifetime);
                    isRun = false;
                    MultiThreadTester.Join();
                }
                return totalCount;

            }
        }

        class ObjectPool<T> where T : class
        {
            private readonly Stack<T> _pool;
            private readonly Func<T> _newFunc;

            public ObjectPool(int size, Func<T> newFunc)
            {
                _pool = new Stack<T>(size);
                _newFunc = newFunc;
                for (int i = 0; i < size; i++)
                {
                    _pool.Push(newFunc());
                }
            }

            public T Rent()
            {
                if (_pool.Count > 0)
                {
                    return _pool.Pop();
                }

                return _newFunc();
            }

            public void Return(T obj)
            {
                _pool.Push(obj);
            }
        }

        class LockedObjectPool<T> where T : class
        {
            private readonly Stack<T> _pool;
            private readonly Func<T> _newFunc;

            public LockedObjectPool(int size, Func<T> newFunc)
            {
                _pool = new Stack<T>(size);
                _newFunc = newFunc;
                for (int i = 0; i < size; i++)
                {
                    _pool.Push(newFunc());
                }
            }

            public T Rent()
            {
                lock (_pool)
                {
                    if (_pool.Count > 0)
                    {
                        return _pool.Pop();
                    }
                }

                return _newFunc();
            }

            public void Return(T obj)
            {
                lock (_pool)
                {
                    _pool.Push(obj);
                }
            }
        }
    }
}
