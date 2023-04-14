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
    public class VariableAccessSpeed : TimeChecker
    {
        static int LoopCount = 100000000;

        int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        int e = 0;
        int f = 0;
        int g = 0;
        int h = 0;
        int i = 0;
        int j = 0;
        int k = 0;

        static int _a = 0;
        static int _b = 0;
        static int _c = 0;
        static int _d = 0;
        static int _e = 0;
        static int _f = 0;
        static int _g = 0;
        static int _h = 0;
        static int _i = 0;
        static int _j = 0;
        static int _k = 0;

        public VariableAccessSpeed()
        {
            int aa = 1;
            int bb = 1;
            int cc = 1;
            int dd = 1;
            int ee = 1;
            int ff = 1;
            int gg = 1;
            int hh = 1;
            int ii = 1;
            int jj = 1;
            int kk = 1;


            while (true)
            {
                BeginSample("Void()");
                for (int i = 0; i < LoopCount; i++) { Void(); }
                EndSample();
                BeginSample("VoidArgument()");
                for (int i = 0; i < LoopCount; i++) { VoidArgument(aa); }
                EndSample();
                BeginSample("VoidMember()");
                for (int i = 0; i < LoopCount; i++) { VoidMember(); }
                EndSample();
                BeginSample("VoidStatic()");
                for (int i = 0; i < LoopCount; i++) { VoidMember(); }
                EndSample();
                BeginSample("Return()");
                for (int i = 0; i < LoopCount; i++) { aa = Return(); }
                EndSample();
                BeginSample("ReturnArgument()");
                for (int i = 0; i < LoopCount; i++) { aa = ReturnArgument(aa); }
                EndSample();
                BeginSample("ReturnMember()");
                for (int i = 0; i < LoopCount; i++) { aa = ReturnMember(); }
                EndSample();
                BeginSample("ReturnStatic()");
                for (int i = 0; i < LoopCount; i++) { aa = ReturnMember(); }
                EndSample();
                BeginSample("RefArgument()");
                for (int i = 0; i < LoopCount; i++) { RefArgument(ref aa); }
                EndSample();
                BeginSample("OutArgument()");
                for (int i = 0; i < LoopCount; i++) { OutArgument(out aa); }
                EndSample();

                BeginSample("VoidArgument_LA()");
                for (int i = 0; i < LoopCount; i++) { VoidArgument_LA(aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk); }
                EndSample();
                BeginSample("VoidMember_LA()");
                for (int i = 0; i < LoopCount; i++) { VoidMember_LA(); }
                EndSample();
                BeginSample("VoidStatic_LA()");
                for (int i = 0; i < LoopCount; i++) { VoidMember_LA(); }
                EndSample();
                BeginSample("RefArgument_LA()");
                for (int i = 0; i < LoopCount; i++) { RefArgument_LA(ref aa, ref bb, ref cc, ref dd, ref ee, ref ff, ref gg, ref hh, ref ii, ref jj, ref kk); }
                EndSample();
                BeginSample("OutArgument_LA()");
                for (int i = 0; i < LoopCount; i++) { OutArgument_LA(out aa, out bb, out cc, out dd, out ee, out ff, out gg, out hh, out ii, out jj, out kk); }
                EndSample();


                PrintSamples();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Void()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void VoidArgument(int b)
        {
            ++b;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void VoidMember()
        {
            ++a;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void VoidStatic()
        {
            ++_a;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int Return()
        {
            return 1;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int ReturnArgument(int b)
        {
            return ++b;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int ReturnMember()
        {
            return ++a;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int ReturnStatic()
        {
            return ++_a;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RefArgument(ref int a)
        {
            ++a;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void OutArgument(out int a)
        {
            a = 1;
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public void VoidArgument_LA(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k)
        {
            a++;
            b++;
            c++;
            d++;
            e++;
            f++;
            g++;
            h++;
            i++;
            j++;
            k++;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void VoidMember_LA()
        {
            a++;
            b++;
            c++;
            d++;
            e++;
            f++;
            g++;
            h++;
            i++;
            j++;
            k++;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void VoidStaticr_LA()
        {
            _a++;
            _b++;
            _c++;
            _d++;
            _e++;
            _f++;
            _g++;
            _h++;
            _i++;
            _j++;
            _k++;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RefArgument_LA(ref int a, ref int b, ref int c, ref int d, ref int e, ref int f, ref int g, ref int h, ref int i, ref int j, ref int k)
        {
            a++;
            b++;
            c++;
            d++;
            e++;
            f++;
            g++;
            h++;
            i++;
            j++;
            k++;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void OutArgument_LA(out int a, out int b, out int c, out int d, out int e, out int f, out int g, out int h, out int i, out int j, out int k)
        {
            a = 1;
            b = 2;
            c = 3;
            d = 54;
            e = 6;
            f = 7;
            g = 8;
            h = 9;
            i = 10;
            j = 11;
            k = 12;
        }

    }
}
