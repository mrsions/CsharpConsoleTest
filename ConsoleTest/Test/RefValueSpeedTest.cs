using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class RefValueSpeedTest : TimeChecker
    {
        int b = 0;
        static int c = 0;

        public RefValueSpeedTest()
        {
            //    int cnt = 100000000;

            //    Debug.Assert(b.IsBit(1) == false);
            //    b.SetBit(1, true);
            //    Debug.Assert(b.IsBit(1) == false);
            //    b = b.SetBit(1, true);
            //    Debug.Assert(b.IsBit(1) == true);
            //    Debug.Assert(b.IsBit(2) == false);
            //    b.SetBitRef(2, true);
            //    Debug.Assert(b.IsBit(2) == true);
            //    b.SetBitRef(1, false);
            //    b.SetBitRef(2, false);

            //    Debug.Assert(c.IsBit(1) == false);
            //    c.SetBit(1, true);
            //    Debug.Assert(c.IsBit(1) == false);
            //    c = c.SetBit(1, true);
            //    Debug.Assert(c.IsBit(1) == true);
            //    Debug.Assert(c.IsBit(2) == false);
            //    c.SetBitRef(2, true);
            //    Debug.Assert(c.IsBit(2) == true);
            //    c.SetBitRef(1, false);
            //    c.SetBitRef(2, false);

            //    while (true)
            //    {
            //        int a = 0;

            //        Debug.Assert(a.IsBit(1) == false);
            //        a.SetBit(1, true);
            //        Debug.Assert(a.IsBit(1) == false);
            //        a = a.SetBit(1, true);
            //        Debug.Assert(a.IsBit(1) == true);
            //        Debug.Assert(a.IsBit(2) == false);
            //        a.SetBitRef(2, true);
            //        Debug.Assert(a.IsBit(2) == true);
            //        a.SetBitRef(1, false);
            //        a.SetBitRef(2, false);

            //        BeginSample("A");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            a = a.SetBit(6, true);
            //        }
            //        EndSample();

            //        BeginSample("B");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            a.SetBitRef(5, true);
            //        }
            //        EndSample();

            //        BeginSample("C");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            a.IsBit(6);
            //        }
            //        EndSample();

            //        BeginSample("D");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            a.IsBitRef(5);
            //        }
            //        EndSample();

            //        BeginSample("A2");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            this.b = this.b.SetBit(6, true);
            //        }
            //        EndSample();

            //        BeginSample("B2");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            this.b.SetBitRef(5, true);
            //        }
            //        EndSample();

            //        BeginSample("C2");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            this.b.IsBit(6);
            //        }
            //        EndSample();

            //        BeginSample("D2");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            this.b.IsBitRef(5);
            //        }
            //        EndSample();

            //        BeginSample("A3");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            c = c.SetBit(6, true);
            //        }
            //        EndSample();

            //        BeginSample("B3");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            c.SetBitRef(5, true);
            //        }
            //        EndSample();

            //        BeginSample("C3");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            c.IsBit(6);
            //        }
            //        EndSample();

            //        BeginSample("D3");
            //        for (int i = 0; i < cnt; i++)
            //        {
            //            c.IsBitRef(5);
            //        }
            //        EndSample();

            //        PrintSamples();
            //    }
        }
    }
}
