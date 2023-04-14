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
    public class SortDelegateTest : TimeChecker
    {
        Comparison<int> compm = (a, b) => a - b;
        static Comparison<int> comps = (a, b) => a - b;
        static int Comps(int a, int b) => a - b;

        Comparison<int> comp2m = (a, b) => a - b - ccc;
        static Comparison<int> comp2s = (a, b) => a - b - ccc;
        int Comp2s(int a, int b) => a - b - cc;

        static Comp icomp = new Comp();
        class Comp : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x - y;
            }
        }

        int cc;
        static int ccc;

        public SortDelegateTest()
        {
            int count = 100000;
            List<int> v = new List<int>(Enumerable.Range(0, 100));

            while (true)
            {
                BeginSample("A");
                for (int i = 0; i < count; i++)
                {
                    v.Sort((a, b) => a - b);
                }
                EndSample();
                BeginSample("B");
                Comparison<int> comp = (a, b) => a - b;
                for (int i = 0; i < count; i++)
                {
                    v.Sort(comp);
                }
                EndSample();
                BeginSample("C");
                for (int i = 0; i < count; i++)
                {
                    Comparison<int> compl = (a, b) => a - b;
                    v.Sort(compl);
                }
                EndSample();
                BeginSample("D");
                for (int i = 0; i < count; i++)
                {
                    v.Sort(compm);
                }
                EndSample();
                BeginSample("E");
                for (int i = 0; i < count; i++)
                {
                    v.Sort(comps);
                }
                EndSample();
                BeginSample("F");
                for (int i = 0; i < count; i++)
                {
                    v.Sort(Comps);
                }
                EndSample();
                BeginSample("G");
                for (int i = 0; i < count; i++)
                {
                    v.Sort(icomp);
                }
                EndSample();


                BeginSample("AA");
                for (int i = 0; i < count; i++)
                {
                    v.Sort((a, b) => a - b - cc);
                }
                EndSample();
                BeginSample("BB");
                Comparison<int> comp2 = (a, b) => a - b - cc;
                for (int i = 0; i < count; i++)
                {
                    v.Sort(comp2);
                }
                EndSample();
                BeginSample("CC");
                for (int i = 0; i < count; i++)
                {
                    Comparison<int> comp2l = (a, b) => a - b - cc;
                    v.Sort(comp2l);
                }
                EndSample();
                BeginSample("DD");
                for (int i = 0; i < count; i++)
                {
                    v.Sort(comp2m);
                }
                EndSample();
                BeginSample("EE");
                for (int i = 0; i < count; i++)
                {
                    v.Sort(comp2s);
                }
                EndSample();
                BeginSample("FF");
                for (int i = 0; i < count; i++)
                {
                    v.Sort(Comp2s);
                }
                EndSample();

                PrintSamples();
            }
        }
    }
}