using ConsoleTest.Benchmark;
using LitJson;
using Realwith.NetPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    public static void Main()
    {
        //using var wr = File.OpenWrite("d:/usage.txt");
        //using var writer = new StreamWriter(wr, Encoding.UTF8);
        //using var reads = File.OpenRead(@"C:\Users\mrsio\OneDrive\바탕 화면\230422-pinoplay.sql");
        //using var reader = new StreamReader(reads, Encoding.UTF8);
        //string line;
        //while ((line = reader.ReadLine()) != null)
        //{
        //    if (line.Contains("://metaversey.") || line.Contains("://rwayoonsblobstorage"))
        //    {
        //        Console.WriteLine(line);
        //        writer.WriteLine(line);
        //    }
        //}
        //MultiThreadTester.Init(100);

        //int threadcount = 1;
        //for (; ; )
        //{
        //    MultiThreadTester.Init(threadcount);
        //    List<double> times = new List<double>(threadcount);

        //    var st = DateTime.Now;
        //    for (int k = 0; k < 10; k++)
        //    {
        //        MultiThreadTester.Start(() =>
        //        {
        //            Call();
        //        });
        //        MultiThreadTester.Join();
        //    }
        //    var mt = DateTime.Now;
        //    for (int k = 0; k < 10; k++)
        //    {
        //        for (int j = 0; j < threadcount; j++)
        //        {
        //            Call();
        //        }
        //    }
        //    var et = DateTime.Now;
        //    Console.WriteLine("end1 " + (mt - st).TotalMilliseconds);
        //    Console.WriteLine("end2 " + (et - mt).TotalMilliseconds);
        //    threadcount = Console.ReadLine().ToInt();
        //}


        Bench_TBT_InstanceVsPool.Benchmark();
        Console.WriteLine("end");
    }

    private static int v;
    private static int Call()
    {
        for (int i = 0; i < 10000000; i++)
        {
            v++;
        }

        return v;
    }
}