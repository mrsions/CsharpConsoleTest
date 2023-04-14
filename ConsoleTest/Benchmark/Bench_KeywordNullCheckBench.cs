using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

namespace ConsoleTest.Benchmark
{
    //[SimpleJob(RunStrategy.ColdStart, launchCount:1, warmupCount:2, targetCount:10, invocationCount:10)]
    [SimpleJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class Bench_KeywordNullCheckBench
    {
        public static void Benchmark()
        {
            BenchmarkRunner.Run<Bench_KeywordNullCheckBench>();
        }

        public static void Test()
        {
            var bench = new Bench_KeywordNullCheckBench();
            bench.Setup();
            //bench.Run11();
            //bench.Run12();
            //bench.Run13();
            //bench.Run14();
            //bench.Run15();
            //bench.Run16();
            //bench.Run21();
            //bench.Run22();
            //bench.Run23();
            //bench.Run24();
            //bench.Run25();
            //bench.Run26();
            bench.Run31();
            bench.Run32();
            bench.Run33();
            bench.Run34();
            bench.Run35();
            bench.Run36();
            bench.Run41();
            bench.Run42();
            bench.Run43();
            bench.Run44();
            bench.Run45();
            bench.Run46();
        }

        public class A { public B? val; }
        public class B { public C? val; }
        public class C { public D? val; }
        public class D { public E? val; }
        public class E { public string? val; }

        private A case1;
        private A case2;
        private A case3;
        private A case4;
        private A case5;
        private A case6;

        private volatile int[] ct = new int[10];

        [GlobalSetup]
        public void Setup()
        {
            case1 = new() { };
            case2 = new() { val = new() { } };
            case3 = new() { val = new() { val = new() { } } };
            case4 = new() { val = new() { val = new() { val = new() { } } } };
            case5 = new() { val = new() { val = new() { val = new() { val = new() { } } } } };
            case6 = new() { val = new() { val = new() { val = new() { val = new() { val = "vv" } } } } };
        }

        //[Benchmark] public void Run11() => Run1(case1);
        //[Benchmark] public void Run12() => Run1(case2);
        //[Benchmark] public void Run13() => Run1(case3);
        //[Benchmark] public void Run14() => Run1(case4);
        //[Benchmark] public void Run15() => Run1(case5);
        //[Benchmark] public void Run16() => Run1(case6);
        //[Benchmark] public void Run21() => Run2(case1);
        //[Benchmark] public void Run22() => Run2(case2);
        //[Benchmark] public void Run23() => Run2(case3);
        //[Benchmark] public void Run24() => Run2(case4);
        //[Benchmark] public void Run25() => Run2(case5);
        //[Benchmark] public void Run26() => Run2(case6);
        [Benchmark] public void Run31() => Run3(case1);
        [Benchmark] public void Run32() => Run3(case2);
        [Benchmark] public void Run33() => Run3(case3);
        [Benchmark] public void Run34() => Run3(case4);
        [Benchmark] public void Run35() => Run3(case5);
        [Benchmark] public void Run36() => Run3(case6);
        [Benchmark] public void Run41() => Run4(case1);
        [Benchmark] public void Run42() => Run4(case2);
        [Benchmark] public void Run43() => Run4(case3);
        [Benchmark] public void Run44() => Run4(case4);
        [Benchmark] public void Run45() => Run4(case5);
        [Benchmark] public void Run46() => Run4(case6);

        public void Run1(A a)
        {
            if (a == null) { ct[0]++; return; }
            if (a.val == null) { ct[1]++; return; }
            if (a.val.val == null) { ct[2]++; return; }
            if (a.val.val.val == null) { ct[3]++; return; }
            if (a.val.val.val.val == null) { ct[4]++; return; }
            if (a.val.val.val.val.val == null) { ct[5]++; return; }

            ct[6]++;
        }

        public void Run2(A a)
        {
            if (a == null) { ct[0]++; return; }
            var a1 = a.val; if (a1 == null) { ct[1]++; return; }
            var a2 = a1.val; if (a2 == null) { ct[2]++; return; }
            var a3 = a2.val; if (a3 == null) { ct[3]++; return; }
            var a4 = a3.val; if (a4 == null) { ct[4]++; return; }
            var a5 = a4.val; if (a5 == null) { ct[5]++; return; }

            ct[6]++;
        }

        public void Run3(A a)
        {
            if (a == null) { ct[0]++; return; }
            if (a.val is not B a1) { ct[1]++; return; }
            if (a1.val is not C a2) { ct[2]++; return; }
            if (a2.val is not D a3) { ct[3]++; return; }
            if (a3.val is not E a4) { ct[4]++; return; }
            if (a4.val is not string a5) { ct[5]++; return; }

            ct[6]++;
        }

        public void Run4(A a)
        {
            if (a == null
                || a.val is not B a1
                || a1.val is not C a2
                || a2.val is not D a3
                || a3.val is not E a4
                || a4.val is not string a5) { ct[5]++; return; }

            ct[6]++;
        }
    }
}
