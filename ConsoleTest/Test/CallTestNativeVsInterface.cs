using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class CallTestNativeVsInterface : TimeChecker
    {
        public CallTestNativeVsInterface()
        {
            int lcnt = 100000000;
            int hcnt = 3000000;
            Caller caller = new Caller();
            ICaller icaller = caller;
            Func<int> nHeavy = caller.Heavy;
            Func<int> nLight = caller.Light;
            Func<int> iHeavy = icaller.Heavy;
            Func<int> iLight = icaller.Light;

            while (true)
            {
                int r = 0;
                BeginSample("Native Light"); for (int i = 0; i < lcnt; i++) r += caller.Light(); EndSample();
                BeginSample("Interf Light"); for (int i = 0; i < lcnt; i++) r += icaller.Light(); EndSample();
                BeginSample("NatDel Light"); for (int i = 0; i < lcnt; i++) r += nLight(); EndSample();
                BeginSample("IntDel Light"); for (int i = 0; i < lcnt; i++) r += iLight(); EndSample();

                BeginSample("Native Heavy"); for (int i = 0; i < hcnt; i++) r += caller.Heavy(); EndSample();
                BeginSample("Interf Heavy"); for (int i = 0; i < hcnt; i++) r += icaller.Heavy(); EndSample();
                BeginSample("NatDel Heavy"); for (int i = 0; i < hcnt; i++) r += nHeavy(); EndSample();
                BeginSample("IntDel Heavy"); for (int i = 0; i < hcnt; i++) r += iHeavy(); EndSample();

                BeginSample("Native? Light"); for (int i = 0; i < lcnt; i++) caller?.Light(); EndSample();
                BeginSample("Interf? Light"); for (int i = 0; i < lcnt; i++) icaller?.Light(); EndSample();
                BeginSample("NatDel? Light"); for (int i = 0; i < lcnt; i++) nLight?.Invoke(); EndSample();
                BeginSample("IntDel? Light"); for (int i = 0; i < lcnt; i++) iLight?.Invoke(); EndSample();

                BeginSample("Native? Heavy"); for (int i = 0; i < hcnt; i++) caller?.Heavy(); EndSample();
                BeginSample("Interf? Heavy"); for (int i = 0; i < hcnt; i++) icaller?.Heavy(); EndSample();
                BeginSample("NatDel? Heavy"); for (int i = 0; i < hcnt; i++) nHeavy?.Invoke(); EndSample();
                BeginSample("IntDel? Heavy"); for (int i = 0; i < hcnt; i++) iHeavy?.Invoke(); EndSample();

                Console.WriteLine(r);
                PrintSamples();
            }
        }
    }

    public class Caller : ICaller
    {
        public int Heavy()
        {
            float v = DateTime.Now.Ticks * 57.5f;
            return (int)MathF.Sin(MathF.Abs(MathF.Atan(MathF.Cos(v))));
        }

        public int Light()
        {
            return 5;
        }
    }

    public interface ICaller
    {
        int Heavy();
        int Light();
    }
}
