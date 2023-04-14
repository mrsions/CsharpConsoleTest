using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleTest
{
    public class SqrtTest
    {
        public static void Run()
        {
            for(int i=10; i<1000;i++)
            {
                int v = SqrtInt(i * i);
                if(v != i)
                {
                    Console.WriteLine($"{i,1000} : {v}");
                }
            }
        }

        public static float Sqrt2(float k)
        {
            float temp = 1;
            float x = 1;

            while (true)
            {
                temp = x;
                x = (x + (k / x)) / 2;

                if (Abs(temp - x) < float.Epsilon)
                {
                    return x;
                }
            }
        }

        public static int SqrtInt(int k)
        {
            int temp = 1;
            int x = 1;

            while (true)
            {
                temp = x;
                x = (x + (k / x)) / 2;

                if (Abs(temp - x) < 1)
                {
                    return x;
                }
            }
        }

        public static float Sqrt(float k)
        {
            float x = 1;
            float next_x;

            if (k < 0)
            {
                return -1;
            }

            while (true)
            {
                next_x = (x / 2) + (k / (2 * x));
                if (Abs(x - next_x) < float.Epsilon)
                {
                    return x;
                }
                x = next_x;
            }
        }

        private static float Abs(float v)
        {
            return v < 0 ? -v : v;
        }
    }
}
