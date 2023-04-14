using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ConsoleTest.Test
{
    public struct VV
    {
        long v;
    }
    public class ArrayCopySpeedTest : TimeChecker
    {
        public ArrayCopySpeedTest()
        {
            VV[] data = new VV[1024 * 1024];
            int count = 1000;
            while (true)
            {
                BeginSample("A");
                for (int i = 0; i < count; i++)
                {
                    data.CloneArray();
                }
                EndSample();
                BeginSample("B");
                for (int i = 0; i < count; i++)
                {
                    data.CloneArray(0, data.Length);
                }
                EndSample();
                BeginSample("C");
                for (int i = 0; i < count; i++)
                {
                    data.CloneArray(5, data.Length - 5);
                }
                EndSample();
                BeginSample("D");
                for (int i = 0; i < count; i++)
                {
                    data.CloneArray(10240, data.Length - 10240);
                }
                EndSample();
                PrintSamples();
            }
        }
    }
}
