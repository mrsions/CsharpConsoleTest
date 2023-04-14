using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleTest.Test
{
    public class LinqSpeedTest
    {
        public static void Run()
        {
            while (true)
            {
                var range = Enumerable.Range(0, 100);
                int ct = 300000;
                int sum = 0;

                Stopwatch st = Stopwatch.StartNew();
                for (int i = 0; i < ct; i++)
                {
                    sum = range.Select(v => v).Where(v=>v<ct).OrderBy(v=>v).Sum();
                }
                st.Stop();

                Stopwatch st2 = Stopwatch.StartNew();
                for (int i = 0; i < ct; i++)
                {
                    sum = (from v in range
                           where v < ct
                           orderby v
                           select v).Sum();
                }
                st2.Stop();

                Console.Clear();
                Console.WriteLine(st.ElapsedMilliseconds);
                Console.WriteLine(st2.ElapsedMilliseconds);
            }
        }

    }
}
