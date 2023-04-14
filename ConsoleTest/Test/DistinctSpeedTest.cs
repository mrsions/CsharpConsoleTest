using SFramework;
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
    public class DistinctSpeedTest : TimeChecker
    {
        public class Temp
        {
        }

        int count = 16 * 4 * 4 * 4 * 4 * 4 * 4 * 4 * 2;

        public DistinctSpeedTest()
        {
            List<Temp> before = new System.Collections.Generic.List<Temp>();
            List<Temp>[] arrays = Enumerable.Range(0, 11).Select(v => new List<Temp>()).ToArray();

            Random r = new Random(100);
            for (int i = 0; i < 1250 / 2 / 2 / 2 / 2 / 2 / 2 / 2 ; i++)
            {
                var n = new Temp();
                before.Add(n);

                var v = (int)(r.NextDouble() * arrays.Length);
                for (int j = 0; j < arrays.Length; j++)
                {
                    if (j <= v)
                    {
                        arrays[j].Add(n);
                    }
                }
            }

            while (true)
            {
                for (int i = 0; i <= 10; i++)
                {
                    var bef = before;
                    var arr = arrays[i];

                    int a = 0;
                    int b = 0;
                    int c = 0;
                    int d = 0;

                    Proc($"[{i}] D({arr.Count}/{bef.Count})", count, () =>
                    {
                        using var p = bef.Except(arr).ToPList();
                        a = p.Count;
                    });

                    Proc($"[{i}] H({arr.Count}/{bef.Count})", count, () =>
                    {
                        using var p = PList.Take<Temp>();
                        var set = new HashSet<Temp>();

                        for (int i = 0, iLen = arr.Count; i < iLen; i++)
                        {
                            set.Add(arr[i]);
                        }

                        for (int i = 0, iLen = bef.Count; i < iLen; i++)
                        {
                            if (set.Add(bef[i])) p.Add(bef[i]);
                        }

                        b = p.Count;
                    });

                    Proc($"[{i}] L({arr.Count}/{bef.Count})", count, () =>
                    {
                        using var p = bef.Where(v => !arr.Contains(v)).ToPList();
                        c = p.Count;
                    });

                    Proc($"[{i}] N({arr.Count}/{bef.Count})", count, () =>
                    {
                        using var p = PList.Take<Temp>();
                        for (int i = 0, iLen = bef.Count; i < iLen; i++)
                        {
                            if (!arr.Contains(bef[i])) p.Add(bef[i]);
                        }

                        d = p.Count;
                    });

                    if (a != b || b != c || c != d)
                    {
                        PrintSamples();
                        Console.WriteLine($"Not Validate {a} {b} {c} {d}");
                        return;
                    }
                }

                PrintSamples();
            }
        }
    }
}
