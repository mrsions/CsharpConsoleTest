using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

namespace ConsoleTest.Benchmark
{
    //[SimpleJob(RunStrategy.ColdStart, launchCount:1, warmupCount:2, targetCount:10, invocationCount:10)]
    [SimpleJob]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class Bench_NintVSint
    {
        public static void Benchmark()
        {
            BenchmarkRunner.Run<Bench_NintVSint>();
        }

        [Benchmark]
        public void Nint()
        {
            nint a0 = 0;
            nint a1 = 1;
            nint a2 = 2;
            nint a3 = 3;
            nint a4 = 4;
            nint a5 = 5;
            nint a6 = 6;
            nint a7 = 7;

            for (nint i = 0; i < 1000; i++)
            {
                nint b0 = 0;
                nint b1 = 1;
                nint b2 = 2;
                nint b3 = 3;
                nint b4 = 4;
                nint b5 = 5;
                nint b6 = 6;
                nint b7 = 7;

                b0++;
                b1++;
                b2++;
                b3++;
                b4++;
                b5++;
                b6++;
                b7++;

                b0 += b1;
                b0 -= b2;
                b0 *= b3;
                b0 /= b4 > 1 ? b4 : 1;
                b0 &= b5;
                b0 |= b6;
                b0 ^= b7;
                b0 <<= (int)a1;
                b0 >>= (int)a2;
                b0 %= a3;

                a0 += b1;
                a0 -= b2;
                a0 *= b3;
                a0 /= b4 > 1 ? b4 : 1;
                a0 &= b5;
                a0 |= b6;
                a0 ^= b7;
                a0 <<= (int)a4;
                a0 >>= (int)a5;
                a0 %= a6 * a7;

                if (a0 == b0)
                {
                    a0++;
                    a1++;
                    a2++;
                    a3++;
                    a4++;
                    a5++;
                    a6++;
                    a7++;
                }
                else
                {
                    a0--;
                    a1--;
                    a2--;
                    a3--;
                    a4--;
                    a5--;
                    a6--;
                    a7--;
                }
            }
        }

        [Benchmark]
        public void Int()
        {
            int a0 = 0;
            int a1 = 1;
            int a2 = 2;
            int a3 = 3;
            int a4 = 4;
            int a5 = 5;
            int a6 = 6;
            int a7 = 7;

            for (int i = 0; i < 1000; i++)
            {
                int b0 = 0;
                int b1 = 1;
                int b2 = 2;
                int b3 = 3;
                int b4 = 4;
                int b5 = 5;
                int b6 = 6;
                int b7 = 7;

                b0++;
                b1++;
                b2++;
                b3++;
                b4++;
                b5++;
                b6++;
                b7++;

                b0 += b1;
                b0 -= b2;
                b0 *= b3;
                b0 /= b4;
                b0 &= b5;
                b0 |= b6;
                b0 ^= b7;
                b0 <<= (int)a1;
                b0 >>= (int)a2;
                b0 %= a3;

                a0 += b1;
                a0 -= b2;
                a0 *= b3;
                a0 /= b4;
                a0 &= b5;
                a0 |= b6;
                a0 ^= b7;
                a0 <<= (int)a4;
                a0 >>= (int)a5;
                a0 %= a6 * a7;

                if (a0 == b0)
                {
                    a0++;
                    a1++;
                    a2++;
                    a3++;
                    a4++;
                    a5++;
                    a6++;
                    a7++;
                }
                else
                {
                    a0--;
                    a1--;
                    a2--;
                    a3--;
                    a4--;
                    a5--;
                    a6--;
                    a7--;
                }
            }
        }
    }
}
