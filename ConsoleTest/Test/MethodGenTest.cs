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
using System.Text.RegularExpressions;

namespace ConsoleTest
{

    // Inner:1          outer:100000000      (frame ms)  (total ms)
    //1 Inner3                                163         1148
    //2 Inner2                                165         1151
    //3 Inner                                 164         1152
    //4 Normal                                187         1313
    //5 Rambda                                1329        9367
    //6 Rambda2                               1522        10587

    // Inner:100000000  outer:1              (frame ms)  (total ms)
    //1 Normal                                46          1058
    //2 Inner                                 46          1060
    //3 Inner3                                46          1060
    //4 Inner2                                46          1070
    //5 Rambda                                164         3769
    //6 Rambda2                               232         5318

    // Inner:10000      outer:10000          (frame ms)  (total ms)
    //1 Inner                                 46          1150
    //2 Normal                                47          1152
    //3 Inner2                                46          1152
    //4 Inner3                                46          1152
    //5 Rambda                                168         4125
    //6 Rambda2                               226         5647

    public class MethodGenTest : TimeChecker
    {
        public int outerCount = 10000;
        public int innerCount = 10000;

        public MethodGenTest()
        {
            while (true)
            {
                Proc($"Inner:{innerCount,-10} outer:{outerCount,-10}");
                Process(Normal);
                Process(Inner);
                Process(Inner2);
                Process(Inner3);
                Process(Rambda);
                Process(Rambda2);
                PrintSamples(1000);
            }
        }

        private int Process(Func<int> action)
        {
            BeginSample(action.Method.Name);
            var i = 0;
            for (int j = 0; j < outerCount; j++)
            {
                i += action();
            }
            EndSample();
            return i;
        }
        //---------------------------------------------------------------------
        private int Normal()
        {
            int a = 0;
            for (int i = 0; i < innerCount; i++)
            {
                a += Add(i, a);
            }
            return a;
        }
        private int Add(int a, int b) => a + b;
        //---------------------------------------------------------------------
        private int Inner()
        {
            int a = 0;
            for (int i = 0; i < innerCount; i++)
            {
                a += Add();
                int Add() => i + a;
            }
            return a;
        }
        //---------------------------------------------------------------------
        private int Inner2()
        {
            int a = 0;
            for (int i = 0; i < innerCount; i++)
            {
                a += Add(i);
            }
            return a;

            int Add(int i) => i + a;
        }
        //---------------------------------------------------------------------
        private int Inner3()
        {
            int a = 0;
            int Add(int i) => i + a;

            for (int i = 0; i < innerCount; i++)
            {
                a += Add(i);
            }
            return a;
        }
        //---------------------------------------------------------------------
        private int Rambda()
        {
            int a = 0;
            Func<int, int> Add = (int i) => i + a;

            for (int i = 0; i < innerCount; i++)
            {
                a += Add(i);
            }
            return a;
        }
        //---------------------------------------------------------------------
        private int Rambda2()
        {
            int a = 0;
            Func<int, int> Add = null;

            for (int i = 0; i < innerCount; i++)
            {
                if (Add == null) Add = (int v) => v + a;
                a += Add(i);
            }
            return a;
        }
        //---------------------------------------------------------------------
    }
}