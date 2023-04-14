using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest.Test
{
    public class DividLongTest : TimeChecker
    {
        public DividLongTest()
        {
            int count = 1000000000;

            int v = 7216;

            var t = new TimeSpan(v * TimeSpan.TicksPerSecond);


            while (true)
            {
                long b = new Random().NextInt64();
                long a = new Random().NextInt64();
                while(a > b) a = new Random().NextInt64();


                decimal r1 = (decimal)a / (decimal)b;
                double r2 = (double)a / (double)b;
                float r3 = (float)a / b;
                float r4 = (float)((decimal)a / b);
                float r5 = (float)((double)a / b);



                //if(r1.ToString("f4") != r3.ToString("f4"))
                //{
                //    Console.WriteLine($"{r1:f5}");
                //    Console.WriteLine($"{r2:f5}");
                //    Console.WriteLine($"{r3:f5}");
                //    Console.WriteLine($"{r4:f5}");
                //    Console.WriteLine($"{r5:f5}");
                //    Console.WriteLine($"{r6:f5}");
                //    Console.WriteLine();
                //}

                //BeginSample("Decimal");
                //for (int i = 0; i < count; i++) { r1 = (decimal)a / b; }
                //EndSample();

                BeginSample("DB");
                for (int i = 0; i < count; i++) { r2 = (double)a / b; }
                EndSample();

                BeginSample("FL");
                for (int i = 0; i < count; i++) { r3 = (float)a / b; }
                EndSample();

                //BeginSample("FL2");
                //for (int i = 0; i < count; i++) { r4 = (float)((decimal)a / b); }
                //EndSample();

                BeginSample("FL3");
                for (int i = 0; i < count; i++) { r5 = (float)((double)a / b); }
                EndSample();

                PrintSamples();
            }

        }
    }
}
