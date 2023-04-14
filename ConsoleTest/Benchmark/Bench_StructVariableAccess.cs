using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ConsoleTest.Benchmark
{
    [SimpleJob(RunStrategy.ColdStart, 1, 100, 100, 10000)]
    //[SimpleJob(RunStrategy.Monitoring)]
    //[SimpleJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class Bench_StructVariableAccess
    {
        public static void Benchmark()
        {
            BenchmarkRunner.Run<Bench_StructVariableAccess>();
        }

        public class Parent
        {
            protected int m_Id;
            public virtual int Id
            {
                get => m_Id;
                set => m_Id = value;
            }
        }

        public class ChildOverride : Parent
        {
            public override int Id
            {
                get => base.Id;
                set => base.Id = value;
            }
        }

        public class ChildNew : Parent
        {
            public new int Id
            {
                get => base.Id;
                set => base.Id = value;
            }
        }

        public class ChildRenew : Parent
        {
            public new int Id
            {
                get => m_Id;
                set => m_Id = value;
            }
        }

        public class ChildOverrideRenew : Parent
        {
            public override int Id
            {
                get => m_Id;
                set => m_Id = value;
            }
        }

        private Parent m_Parent = new();
        private ChildOverride m_ChildOverride = new();
        private ChildNew m_ChildNew = new();
        private ChildRenew m_ChildRenew = new();
        private ChildOverrideRenew m_ChildOverrideRenew = new();

        [Benchmark] public void BenchParent() { m_Parent.Id = m_Parent.Id + 1; }
        [Benchmark] public void BenchChildOverride() { m_ChildOverride.Id = m_ChildOverride.Id + 1; }
        [Benchmark] public void BenchChildNew() { m_ChildNew.Id = m_ChildNew.Id + 1; }
        [Benchmark] public void BenchChildRenew() { m_ChildRenew.Id = m_ChildRenew.Id + 1; }
        [Benchmark] public void BenchChildOverrideRenew() { m_ChildOverrideRenew.Id = m_ChildOverrideRenew.Id + 1; }
    }
}
