using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class ConcurrentBagTest : TimeChecker
    {
        static object obj = new object();

        public ConcurrentBagTest()
        {
            const int thread = 5;
            const int count = 1000000;
            int result = 0;


            while (true)
            {
                //BeginSample("ConcurrentQueue");
                //{
                //    ConcurrentQueue<int> bag = new System.Collections.Concurrent.ConcurrentQueue<int>();
                //    int add = thread;
                //    int read = thread;
                //    for (int i = 0; i < thread; i++)
                //    {
                //        new Thread(() =>
                //        {
                //            for (int j = 0; j < count; j++)
                //            {
                //                bag.Enqueue(j);
                //            }
                //            lock (obj)
                //            {
                //                add--;
                //            }
                //        }).Start();
                //        new Thread(() =>
                //        {
                //            int v = 0;
                //            for (int j = 0; j < count; j++)
                //            {
                //                if (bag.TryDequeue(out var r))
                //                {
                //                    v += r;
                //                }
                //            }
                //            result += v;
                //            lock (obj)
                //            {
                //                read--;
                //            }
                //        }).Start();
                //    }
                //    while (add + read == 0) ;
                //}
                //EndSample();
                //BeginSample("Queue");
                //{
                //    int add = thread;
                //    int read = thread;
                //    Queue<int> list = new Queue<int>();
                //    for (int i = 0; i < thread; i++)
                //    {
                //        new Thread(() =>
                //        {
                //            for (int j = 0; j < count; j++)
                //            {
                //                lock (list)
                //                {
                //                    list.Enqueue(j);
                //                }
                //            }
                //            lock (obj)
                //            {
                //                add--;
                //            }
                //        }).Start();
                //        new Thread(() =>
                //        {
                //            int v = 0;
                //            for (int j = 0; j < count; j++)
                //            {
                //                lock (list)
                //                {
                //                    if (list.Count > 0)
                //                    {
                //                        v += list.Dequeue();
                //                    }
                //                }
                //            }
                //            result += v;
                //            lock (obj)
                //            {
                //                read--;
                //            }
                //        }).Start();
                //    }
                //    while (add + read == 0) ;
                //}
                //EndSample();
                BeginSample("ConcurrentDictionary");
                {
                    ConcurrentDictionary<int, int> dict = new ConcurrentDictionary<int, int>();
                    List<Thread> threads = new List<Thread>();
                    for (int i = 0; i < thread; i++)
                    {
                        int tcCount = 0;
                        int tStart = i * count;
                        threads.Add(new Thread(() =>
                        {
                            for (int k = 0; k < count; k++)
                            {
                                int idx = tStart + k;
                                while (!dict.TryAdd(idx, idx))
                                {
                                }
                                tcCount = k;
                            }
                        }));
                        threads.Add(new Thread(() =>
                        {
                            Thread.Sleep(1);
                            Random rnd = new Random();
                            int v = 0, vv;
                            for (int k = 0; k < count; k++)
                            {
                                int idx = tStart + k;
                                while (!dict.TryGetValue(idx, out vv)) ;
                                v += vv;
                            }
                            result += v;
                        }));
                    }
                    foreach (var t in threads) t.Start();
                    foreach (var t in threads) t.Join();
                }
                EndSample();
                BeginSample("Sync Dictionary");
                {
                    Dictionary<int, int> dict = new Dictionary<int, int>();
                    List<Thread> threads = new List<Thread>();
                    for (int i = 0; i < thread; i++)
                    {
                        int tcCount = 0;
                        int tStart = i * count;
                        threads.Add(new Thread(() =>
                        {
                            for (int k = 0; k < count; k++)
                            {
                                int idx = tStart + k;
                                lock (dict)
                                {
                                    dict.Add(idx, idx);
                                }
                                tcCount = k;
                            }
                        }));
                        threads.Add(new Thread(() =>
                        {
                            Thread.Sleep(1);
                            Random rnd = new Random();
                            int v = 0;
                            for (int k = 0; k < count; k++)
                            {
                                int idx = tStart + k;
                                while (true)
                                {
                                    lock (dict)
                                    {
                                        if (dict.ContainsKey(idx))
                                        {
                                            break;
                                        }
                                    }
                                }
                                lock (dict)
                                {
                                    v += dict[idx];
                                }
                            }
                            result += v;
                        }));
                    }
                    foreach (var t in threads) t.Start();
                    foreach (var t in threads) t.Join();
                }
                EndSample();

                BeginSample("ConcurrentDictionary Indexer");
                {
                    ConcurrentDictionary<int, int> dict = new ConcurrentDictionary<int, int>();
                    List<Thread> threads = new List<Thread>();
                    for (int i = 0; i < thread; i++)
                    {
                        int tcCount = 0;
                        int tStart = i * count;
                        threads.Add(new Thread(() =>
                        {
                            for (int k = 0; k < count; k++)
                            {
                                int idx = tStart + k;
                                dict[idx] = idx;
                                tcCount = k;
                            }
                        }));
                        threads.Add(new Thread(() =>
                        {
                            int vv;
                            Thread.Sleep(1);
                            Random rnd = new Random();
                            int v = 0;
                            for (int k = 0; k < count; k++)
                            {
                                int idx = tStart + k;
                                while (!dict.TryGetValue(idx, out vv)) ;
                                v += vv;
                            }
                            result += v;
                        }));
                    }
                    foreach (var t in threads) t.Start();
                    foreach (var t in threads) t.Join();
                }
                EndSample();
                BeginSample("Sync Dictionary Indexer");
                {
                    Dictionary<int, int> dict = new Dictionary<int, int>();
                    List<Thread> threads = new List<Thread>();
                    for (int i = 0; i < thread; i++)
                    {
                        int tcCount = 0;
                        int tStart = i * count;
                        threads.Add(new Thread(() =>
                        {
                            for (int k = 0; k < count; k++)
                            {
                                int idx = tStart + k;
                                lock (dict)
                                {
                                    dict[idx] = (idx);
                                }
                                tcCount = k;
                            }
                        }));
                        threads.Add(new Thread(() =>
                        {
                            Thread.Sleep(1);
                            Random rnd = new Random();
                            int v = 0;
                            for (int k = 0; k < count; k++)
                            {
                                int idx = tStart + k;
                                while (true)
                                {
                                    lock (dict)
                                    {
                                        if (dict.ContainsKey(idx))
                                        {
                                            break;
                                        }
                                    }
                                }
                                lock (dict)
                                {
                                    v += dict[idx];
                                }
                            }
                            result += v;
                        }));
                    }
                    foreach (var t in threads) t.Start();
                    foreach (var t in threads) t.Join();
                }
                EndSample();

                //-------------------------------------------------------------------------------------------------------
                {
                    Dictionary<int, int> dict = new Dictionary<int, int>();
                    BeginSample("Sync Dictionary Add");
                    {
                        List<Thread> threads = new List<Thread>();
                        for (int i = 0; i < thread; i++)
                        {
                            int tcCount = 0;
                            int tStart = i * count;
                            threads.Add(new Thread(() =>
                            {
                                for (int k = 0; k < count; k++)
                                {
                                    int idx = tStart + k;
                                    lock (dict)
                                    {
                                        dict.Add(idx, idx);
                                    }
                                    tcCount = k;
                                }
                            }));
                        }
                        foreach (var t in threads) t.Start();
                        foreach (var t in threads) t.Join();
                    }
                    EndSample();
                    BeginSample("Sync Dictionary Get");
                    {
                        List<Thread> threads = new List<Thread>();
                        for (int i = 0; i < thread; i++)
                        {
                            int tcCount = 0;
                            int tStart = i * count;
                            threads.Add(new Thread(() =>
                            {
                                Thread.Sleep(1);
                                Random rnd = new Random();
                                int v = 0;
                                for (int k = 0; k < count; k++)
                                {
                                    int idx = tStart + k;
                                    while (true)
                                    {
                                        lock (dict)
                                        {
                                            if (dict.ContainsKey(idx))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    lock (dict)
                                    {
                                        v += dict[idx];
                                    }
                                }
                                result += v;
                            }));
                        }
                        foreach (var t in threads) t.Start();
                        foreach (var t in threads) t.Join();
                    }
                    EndSample();
                }

                {
                    ConcurrentDictionary<int, int> dict = new ConcurrentDictionary<int, int>();
                    BeginSample("ConcurrentDictionary Add");
                    {
                        List<Thread> threads = new List<Thread>();
                        for (int i = 0; i < thread; i++)
                        {
                            int tcCount = 0;
                            int tStart = i * count;
                            threads.Add(new Thread(() =>
                            {
                                for (int k = 0; k < count; k++)
                                {
                                    int idx = tStart + k;
                                    //while (!dict.TryAdd(idx, idx)) ;
                                    dict[idx] = idx;
                                    tcCount = k;
                                }
                            }));
                        }
                        foreach (var t in threads) t.Start();
                        foreach (var t in threads) t.Join();
                    }
                    EndSample();
                    BeginSample("ConcurrentDictionary Get");
                    {
                        List<Thread> threads = new List<Thread>();
                        for (int i = 0; i < thread; i++)
                        {
                            int tcCount = 0;
                            int tStart = i * count;
                            threads.Add(new Thread(() =>
                            {
                                Thread.Sleep(1);
                                Random rnd = new Random();
                                int v = 0, vv;
                                for (int k = 0; k < count; k++)
                                {
                                    int idx = tStart + k;
                                    v += dict[idx];
                                }
                                result += v;
                            }));
                        }
                        foreach (var t in threads) t.Start();
                        foreach (var t in threads) t.Join();
                    }
                    EndSample();
                }
                PrintSamples();
            }
        }


    }
}
