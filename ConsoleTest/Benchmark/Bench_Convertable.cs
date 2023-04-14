using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ConsoleTest.Benchmark
{
    [SimpleJob(RunStrategy.ColdStart, 1, 1000, 1000, 10000)]
    //[SimpleJob(RunStrategy.Monitoring)]
    //[SimpleJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class Bench_Convertable
    {
        public static void Benchmark()
        {
            BenchmarkRunner.Run<Bench_Convertable>();
        }

        [Benchmark] public void BenchNativedouble() { AddNaive((double)10, (double)22); }
        [Benchmark] public void BenchNativeint() { AddNaive((int)10, (int)22); }
        [Benchmark] public void BenchNativelong() { AddNaive((long)10, (long)22); }
        [Benchmark] public void BenchNativebyte() { AddNaive((byte)10, (byte)22); }

        [Benchmark] public void BenchCastdouble() { Adddouble((double)10, (double)22); }
        [Benchmark] public void BenchCastint() { Addint((int)10, (int)22); }
        [Benchmark] public void BenchCastlong() { Addlong((long)10, (long)22); }
        [Benchmark] public void BenchCastbyte() { Addbyte((byte)10, (byte)22); }


        public double Adddouble<T>(T a, T b)
            where T : IConvertible
        {
            return a.ToDouble(null) + b.ToDouble(null);
        }
        public int Addint<T>(T a, T b)
            where T : IConvertible
        {
            return a.ToInt32(null) + b.ToInt32(null);
        }
        public long Addlong<T>(T a, T b)
            where T : IConvertible
        {
            return a.ToInt64(null) + b.ToInt64(null);
        }
        public byte Addbyte<T>(T a, T b)
            where T : IConvertible
        {
            return (byte)(a.ToByte(null) + b.ToByte(null));
        }

        public double AddNaive(double a, double b) => a + b;
        public int AddNaive(int a, int b) => a + b;
        public long AddNaive(long a, long b) => a + b;
        public byte AddNaive(byte a, byte b) => (byte)(a + b);
    }
}
