//Path Time        TotalTime   Call               Alloc  Alloc Total
//Int 101           13          801         61                    7b         432b
//Str 101           22          1370        61                    0b          56b
//Int 102           13          800         61                    0b           0b
//Str 102           22          1371        61                    0b           0b
//Int 104           13          804         61                    0b           0b
//Str 104           23          1428        61                    0b           0b
//Int 108           13          800         61                    0b           0b
//Str 108           23          1425        61                    0b           0b
//Int 116           13          803         61                    0b           0b
//Str 116           22          1364        61                    0b           0b
//Int 132           13          805         61                    0b           0b
//Str 132           22          1377        61                    0b           0b
//Int 164           13          811         61                    0b           0b
//Str 164           25          1536        61                    0b           0b
//Int 228           13          797         61                    0b           0b
//Str 228           24          1474        61                    0b           0b
//Int 356           13          804         61                    0b           0b
//Str 356           26          1595        61                    0b           0b
//Int 612           14          858         61                    0b           0b
//Str 612           25          1550        61                    0b           0b
//Int 1124          13          801         61                    0b           0b
//Str 1124          27          1662        61                    0b           0b
//Int 2148          14          872         61                    0b           0b
//Str 2148          28          1722        61                    0b           0b
//Int 4196          14          860         61                    0b           0b
//Str 4196          30          1907        61                    0b           0b
//Int 8292          15          929         61                    0b           0b
//Str 8292          41          2534        61                    0b           0b
//Int 16484         19          1171        61                    0b           0b
//Str 16484         46          2800        61                    0b           0b
//Int 32868         24          1475        61                    0b           0b
//Str 32868         53          3242        61                    0b           0b
//Int 65636         26          1611        61                    0b           0b
//Str 65636         54          3367        61                    0b           0b
//Int 131172        28          1728        61                    0b           0b
//Str 131172        66          4488        61                    0b           0b
//Int 262244        30          1948        61                    0b           0b
//Str 262244        119         8466        61                    0b           0b
//Int 524388        47          3097        61                    0b           0b
//Str 524388        211         12779       61                    0b           0b
//Int 1048676       91          6410        61                    0b           0b
//Str 1048676       251         16378       61                    0b           0b
//Int 2097252       140         9031        61                    0b           0b
//Str 2097252       302         18662       61                    0b           0b
//Int 4194404       181         11019       61                    0b           0b
//Str 4194404       345         21603       61                    0b           0b
//Int 8388708       200         12426       61                    0b           0b
//Str 8388708       351         21439       61                    0b           0b
//Int 16777316      221         13427       61                    0b           0b
//Str 16777316      371         22989       61                    0b           0b
//Int 33554532      214         13362       61                    0b           0b
//Str 33554532      387         24845       61                    0b           0b
//Int 67108964      220         14154       61                    0b           0b
//Str 67108964      453         29400       61                    0b           0b
//Int 134217828     241         15576       61                    0b           0b
//Str 134217828     526         33561       61                    0b           0b

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
    public class DictionarySearchSpeedTest2 : TimeChecker
    {
        Random rnd = new Random();


        int[] dictIntKeys;
        Dictionary<int, int>[] dictInt = new Dictionary<int, int>[28];

        string[] dictStringKeys;
        Dictionary<string, string>[] dictString;

        public long a;
        public string b;

        const int cnt = 1000000;

        public DictionarySearchSpeedTest2()
        {
            dictIntKeys = new int[(int)Math.Pow(2, dictInt.Length - 1) + 100];
            dictStringKeys = new string[dictIntKeys.Length];
            for (int i = 0; i < dictIntKeys.Length; i++)
            {
                dictIntKeys[i] = i;
                dictStringKeys[i] = i.ToString();
                if ((i & 0x3FF) == 0)
                {
                    Console.WriteLine(i + "/" + dictIntKeys.Length);
                }
            }
            dictString = new Dictionary<string, string>[dictInt.Length];
            for (int i = 0; i < dictInt.Length; i++)
            {
                var di = dictInt[i] = new Dictionary<int, int>();
                var ds = dictString[i] = new Dictionary<string, string>();
                for (int k = 0, kLen = (int)Math.Pow(2, i) + 100; k < kLen; k++)
                {
                    di[dictIntKeys[dictIntKeys.Length - k - 1]] = dictIntKeys[k];
                    ds[dictStringKeys[dictIntKeys.Length - k - 1]] = dictStringKeys[k];
                }
                Console.WriteLine(i);
            }

            while (true)
            {
                for (int i = 0; i < dictInt.Length; i++)
                {
                    Proc("Int " + dictInt[i].Count, 1, () =>
                    {
                        var d = dictInt[i];
                        int a = 0;
                        int ed = dictIntKeys.Length;
                        int st = dictIntKeys.Length - d.Count;
                        for (int j = 0; j < cnt; j++)
                        {
                            a += d[dictIntKeys[rnd.Next(st, ed)]];
                        }
                        this.a = a;
                    });
                    Proc("Str " + dictString[i].Count, 1, () =>
                    {
                        var d = dictString[i];
                        string b = null;
                        int ed = dictIntKeys.Length;
                        int st = dictIntKeys.Length - d.Count;
                        for (int j = 0; j < cnt; j++)
                        {
                            b = d[dictStringKeys[rnd.Next(st, ed)]];
                        }
                        this.b = b;
                    });
                }

                Console.WriteLine(a + b);
                PrintSamples();
            }
        }
    }
}
