using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleTest
{
    public class DelegateTest : TimeChecker
    {
        public DelegateTest()
        {
            int count = 100000000;

            while (true)
            {
                Action void0 = Void0;
                Action<int> void1 = Void1;
                Action<int, int> void2 = Void2;
                Func<int> sum0 = Sum0;
                Func<int, int> sum1 = Sum1;
                Func<int, int, int> sum2 = Sum2;
                OAction<int> oSum0 = OSum0;
                OAction<int, int> oSum1 = OSum1;
                OAction<int, int, int> oSum2 = OSum2;

                int a = 0;

                BeginSample("Native Void0"); for (int i = 0; i < count; i++) { Void0(); }
                EndSample();
                BeginSample("Native Void1"); for (int i = 0; i < count; i++) { Void1(i); }
                EndSample();
                BeginSample("Native Void2"); for (int i = 0; i < count; i++) { Void2(i, i); }
                EndSample();
                BeginSample("Native Sum0"); for (int i = 0; i < count; i++) { a += Sum0(); }
                EndSample();
                BeginSample("Native Sum1"); for (int i = 0; i < count; i++) { a += Sum1(i); }
                EndSample();
                BeginSample("Native Sum2"); for (int i = 0; i < count; i++) { a += Sum2(i, i); }
                EndSample();
                BeginSample("Native Out Sum0"); for (int i = 0; i < count; i++) { OSum0(out a); }
                EndSample();
                BeginSample("Native Out Sum1"); for (int i = 0; i < count; i++) { OSum1(out a, i); }
                EndSample();
                BeginSample("Native Out Sum2"); for (int i = 0; i < count; i++) { OSum2(out a, i, i); }
                EndSample();


                BeginSample("Delegate Void0"); for (int i = 0; i < count; i++) { void0(); }
                EndSample();
                BeginSample("Delegate Void1"); for (int i = 0; i < count; i++) { void1(i); }
                EndSample();
                BeginSample("Delegate Void2"); for (int i = 0; i < count; i++) { void2(i, i); }
                EndSample();
                BeginSample("Delegate Sum0"); for (int i = 0; i < count; i++) { a += sum0(); }
                EndSample();
                BeginSample("Delegate Sum1"); for (int i = 0; i < count; i++) { a += sum1(i); }
                EndSample();
                BeginSample("Delegate Sum2"); for (int i = 0; i < count; i++) { a += sum2(i, i); }
                EndSample();
                BeginSample("Delegate Out Sum0"); for (int i = 0; i < count; i++) { oSum0(out a); }
                EndSample();
                BeginSample("Delegate Out Sum1"); for (int i = 0; i < count; i++) { oSum1(out a, i); }
                EndSample();
                BeginSample("Delegate Out Sum2"); for (int i = 0; i < count; i++) { oSum2(out a, i, i); }
                EndSample();

                PrintSamples();
            }
        }

        static int v;

        void Void0()
        {
            v++;
        }

        void Void1(int a)
        {
            v++;
        }

        void Void2(int a, int b)
        {
            v++;
        }

        int Sum0()
        {
            v++;
            return 1;
        }

        int Sum1(int a)
        {
            v++;
            return 1;
        }

        int Sum2(int a, int b)
        {
            v++;
            return 1;
        }

        delegate void OAction<O>(out O o);
        delegate void OAction<O, A>(out O o, A a);
        delegate void OAction<O, A, B>(out O o, A a, B b);
        void OSum0(out int o)
        {
            v++;
            o = 1;
        }

        void OSum1(out int o, int a)
        {
            v++;
            o = 1;
        }

        void OSum2(out int o, int a, int b)
        {
            v++;
            o = 1;
        }
    }
}