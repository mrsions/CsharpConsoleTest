using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ConsoleTest.Benchmark
{
    //[SimpleJob(RunStrategy.ColdStart, 2, 1000, 1000, 100000)]
    [SimpleJob]
    [MemoryDiagnoser]
    public class Bench_DelegateMemoryTest
    {
        public static void Benchmark()
        {
            BenchmarkRunner.Run<Bench_DelegateMemoryTest>();
        }

        public static int StaticSum(int a, int b)
        {
            return a + b;
        }
        public int MemberSum(int a, int b)
        {
            return a + b;
        }

        public static int StaticSum_Static(int a, int b)
        {
            return a + b + s_Field;
        }
        public int MemberSum_Static(int a, int b)
        {
            return a + b + s_Field;
        }

        public int MemberSum_Member(int a, int b)
        {
            return a + b + m_Field;
        }

        private int m_Field;
        private static int s_Field;
        public Func<int, int, int> del_StaticSum ;
        public Func<int, int, int> del_StaticSum_Static ;
        public Func<int, int, int> del_MemberSum ;
        public Func<int, int, int> del_MemberSum_Static ;
        public Func<int, int, int> del_MemberSum_Member;

        public Bench_DelegateMemoryTest()
        {
            del_StaticSum = StaticSum;
            del_StaticSum_Static = StaticSum_Static;
            del_MemberSum = MemberSum;
            del_MemberSum_Static = MemberSum_Static;
            del_MemberSum_Member = MemberSum_Member;
        }

        private static int m_Test = new Random().Next(0, 1000);
        private static int m_Test2 = new Random().Next(0, 1000);
        public void Run(Func<int, int, int>? func)
        {
            if (func != null)
            {
                m_Field += s_Field += func(m_Test, m_Test2);
            }
        }

        //-------------------------------------------------------------------------------
        // 이미 선언된 Method를 참조하는 실행
        [Benchmark] public void Method_Static() { Run(StaticSum); }
        [Benchmark] public void Method_Member() { Run(MemberSum); }
        [Benchmark] public void Method_Static_Static() { Run(StaticSum_Static); }
        [Benchmark] public void Method_Member_Static() { Run(MemberSum_Static); }
        [Benchmark] public void Method_Member_Member() { Run(MemberSum_Member); }

        //-------------------------------------------------------------------------------
        // 이미 선언된 Delegate를 참조하는 실행
        [Benchmark] public void Del_Static() { Run(del_StaticSum); }
        [Benchmark] public void Del_Member() { Run(del_MemberSum); }
        [Benchmark] public void Del_Static_Static() { Run(del_StaticSum_Static); }
        [Benchmark] public void Del_Member_Static() { Run(del_MemberSum_Static); }
        [Benchmark] public void Del_Member_Member() { Run(del_MemberSum_Member); }

        //-------------------------------------------------------------------------------
        // 람다식을 사용한 실행
        [Benchmark] public void Lambda_Static() { Run(static (a, b) => a + b); }
        [Benchmark] public void Lambda_Member() { Run((a, b) => a + b); }
        [Benchmark] public void Lambda_Static_Static() { Run(static (a, b) => a + b + s_Field); }
        [Benchmark] public void Lambda_Member_Static() { Run((a, b) => a + b + s_Field); }
        [Benchmark] public void Lambda_Member_Member() { Run((a, b) => a + b + m_Field); }

        //-------------------------------------------------------------------------------
        // 람다식에서 지역변수를 사용하여 실행
        [Benchmark] public void Lambda_Member_Local() { int c = m_Field; Run((a, b) => a + b + c); }
        [Benchmark] public void Lambda_Member_LocalMember() { int c = m_Field; Run((a, b) => a + b + c + m_Field); }
        [Benchmark] public void Lambda_Member_LocalStatic() { int c = m_Field; Run((a, b) => a + b + c + s_Field); }
        [Benchmark] public void Lambda_Member_LocalMemberStatic() { int c = m_Field; Run((a, b) => a + b + c + m_Field + s_Field); }


    }
}
