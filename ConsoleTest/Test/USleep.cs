using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleTest.Test
{
    public class USleep
    {
        public USleep()
        {
            //for (int i = 0; i < 10; i++)
            {
                new System.Threading.Thread(() =>
                {
                    int v = 0;
                    DateTime dt = DateTime.Now;
                    while ((DateTime.Now - dt).TotalSeconds < 5)
                    {
                        v++;
                        //Sleep.sleepUs(1);
                        //System.Threading.Thread.Sleep(1);
                        //RwSlam.Native.Rwslam_Usleep(1);
                    }
                    Console.WriteLine(v);
                }).Start();
            }

            System.Threading.Thread.Sleep(int.MaxValue);
        }
    }

    public static class Sleep
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private static long freq;
        private static double ufreq;
        private static double mfreq;

        static Sleep()
        {
            if (QueryPerformanceFrequency(out freq) == false)
            {
                throw new Win32Exception();
            }
            ufreq = freq / 1000000;
            mfreq = freq / 1000;
        }

        public static void sleepUs(double us)
        {
            long startTime = 0;
            long nowTime = 0;

            QueryPerformanceCounter(out startTime);

            while (((nowTime - startTime) / ufreq) < us)
            {
                QueryPerformanceCounter(out nowTime);
            }
        }

        public static void sleepMs(double ms)
        {
            long startTime = 0;
            long nowTime = 0;

            QueryPerformanceCounter(out startTime);

            while (((nowTime - startTime) / mfreq) < ms)
            {
                QueryPerformanceCounter(out nowTime);
            }
        }
    }
}
