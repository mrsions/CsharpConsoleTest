using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ConsoleTest
{
    public class TimeChecker
    {
        public class TimeColumn
        {
            public string msg;
            public long time;
            public long total;
            public long count;
            public long gc;
        }

        private Stopwatch st;
        private string msg;
        private List<TimeColumn> times = new List<TimeColumn>();
        private int timeIndex;
        private long startAlloc;
        public bool useBeginLog;
        public bool useClearPrint = true;

        public void Proc(string msg) => Proc(msg, 0, null);
        public void Proc(string msg, int len, Action act)
        {
            BeginSample(msg);
            for(int i=0; i<len; i++)
            {
                act();
            }
            EndSample();
        }

        public void Proc(int len, Action act)
        {
            BeginSample(act.Method.Name);
            for (int i = 0; i < len; i++)
            {
                act();
            }
            EndSample();
        }

        public void BeginSample(string msg)
        {
            if (useBeginLog) Console.WriteLine(msg);
            timeIndex = -1;
            for (int i=0; i<times.Count; i++)
            {
                if(times[i].msg == msg)
                {
                    timeIndex = i;
                    break;
                }
            }
            if (timeIndex == -1)
            {
                timeIndex = times.Count;
                times.Add(new TimeColumn { msg = msg });
            }

            startAlloc = GC.GetAllocatedBytesForCurrentThread();

            st = Stopwatch.StartNew();
        }

        public void EndSample()
        {
            long mills = st.ElapsedMilliseconds;
            times[timeIndex].time = mills;
            times[timeIndex].total += mills;
            times[timeIndex].gc += (GC.GetAllocatedBytesForCurrentThread() - startAlloc - 40);
            times[timeIndex].count++;
        }

        public void ForceGC()
        {
            GC.Collect();
        }

        public void ClearTime()
        {
            foreach (var t in times) t.time = 0;
        }
        public void ClearAll()
        {
            foreach (var t in times)
            {
                t.time = 0;
                t.total = 0;
            }
        }

        public void PrintSamples(int sleep = 100)
        {
            if (useClearPrint)
            {
                Console.WriteLine();
                Console.Clear();
            }
            int maxLen = 0;
            foreach (var t in times) maxLen = Math.Max(maxLen, t.msg.Length);
            maxLen += 4;

            //header
            Console.Write("Path");
            for (int i = 0; i < (maxLen - "Path".Length); i++)
            {
                Console.Write(' ');
            }
            Console.WriteLine($" {"Time",-11} {"TotalTime",-11} {"Call",-11} {"Alloc",12} {"Alloc Total",12}");

            // values
            foreach (var t in times)
            {
                Console.Write(t.msg);
                for(int i=0; i<(maxLen-t.msg.Length); i++)
                {
                    Console.Write(' ');
                }
                Console.WriteLine($" {t.time,-11} {t.total,-11} {t.count, -11} {t.gc/t.count,11:N0}b {t.gc,11:N0}b");
            }
            Thread.Sleep(sleep);
        }
    }
}
