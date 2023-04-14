using System;

namespace ConsoleTest
{
    public class SpanAllocTest : TimeChecker
    {
        public SpanAllocTest()
        {
            const int cnt = 100000;
            int rst = 0;
            while (true)
            {
                BeginSample("A");
                for (int i = 0; i < cnt; i++)
                {
                    rst = Test(i);
                }
                EndSample();
                BeginSample("B");
                for (int i = 0; i < cnt; i++)
                {
                    Test2(i);
                }
                EndSample();
                BeginSample("C");
                for (int i = 0; i < cnt; i++)
                {
                    Test3(i);
                }
                EndSample();
                BeginSample("D");
                for (int i = 0; i < cnt; i++)
                {
                    Test4(i);
                }
                EndSample();


                int Test(int i)
                {
                    Span<int> v = stackalloc int[10000];
                    v.Fill(i);
                    return v[i % 10000];
                }


                int Test2(int i)
                {
                    int[] v = new int[10000];
                    Array.Fill(v, i);
                    return v[i % 10000];
                }


                int Test3(int i)
                {
                    Span<int> v = stackalloc int[10000];
                    for (int j = 0; j < v.Length; j++)
                    {
                        v[j] = v[j] + i;
                    }
                    return v[i % 10000];
                }


                int Test4(int i)
                {
                    int[] v = new int[10000];
                    for (int j = 0; j < v.Length; j++)
                    {
                        v[j] = v[j] + i;
                    }
                    return v[i % 10000];
                }

                //BeginSample("B");
                //for (int i = 0; i < cnt; i++)
                //{
                //    //c = SMath.Round(v);
                //}
                //EndSample();
                //BeginSample("C");
                //for (int i = 0; i < cnt; i++)
                //{
                //    c = SMath.Round2(v, 2);
                //}
                //EndSample();
                //BeginSample("D");
                //for (int i = 0; i < cnt; i++)
                //{
                //    c = SMath.Round3(v, 2);
                //}
                //EndSample();
                //Console.WriteLine("--------------------------" + c);
                PrintSamples();
            }
        }
    }
}
