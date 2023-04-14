using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ConsoleTest.Test
{
    public class NumberIncreaseTest
    {
        int threadCount = 10_000;
        int loopCount = 10_000;

        static object o = new object();
        static int request;
        static int response;

        static int n;
        static int a;
        static int b;
        int c;
        int d;
        int e;

        public NumberIncreaseTest()
        {
            Console.WriteLine($"Star Thread {threadCount}");
            for (int i = 0; i < threadCount; i++)
            {
                new Thread(Running).Start();
            }

            Console.WriteLine($"Wait for processing");
            while (request != response) Thread.Sleep(100);

            Console.WriteLine($"Result");
            Console.WriteLine($"++(static int) ({a} {(a == threadCount * loopCount ? "==" : "!=")} {threadCount * loopCount}");
            Console.WriteLine($"(static int)++ ({b} {(b == threadCount * loopCount ? "==" : "!=")} {threadCount * loopCount}");
            Console.WriteLine($"++(member int) ({c} {(c == threadCount * loopCount ? "==" : "!=")} {threadCount * loopCount}");
            Console.WriteLine($"(member int)++ ({d} {(d == threadCount * loopCount ? "==" : "!=")} {threadCount * loopCount}");
            Console.WriteLine($"d = d+1        ({e} {(e == threadCount * loopCount ? "==" : "!=")} {threadCount * loopCount}");
            Console.WriteLine($"new class      ({n} {(n == threadCount * loopCount ? "==" : "!=")} {threadCount * loopCount}");
        }

        private void Running(object obj)
        {
            lock (o)
            {
                request++;
            }

            while (request != threadCount) Thread.Sleep(1);

            for (int i = 0; i < loopCount; i++)
            {
                ++a;
                b++;
                ++c;
                d++;
                e = e + 1;
                new Temp();
            }

            lock (o)
            {
                response++;
            }
        }

        public class Temp
        {
            public int Id = ++n;
        }
    }
}
