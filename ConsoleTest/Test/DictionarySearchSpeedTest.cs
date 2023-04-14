//Path Time        TotalTime   Call               Alloc  Alloc Total
//Int 101           14          282         20                   21b         432b
//Str 101           22          441         20                    2b          56b
//Int 102           14          282         20                    0b           0b
//Str 102           22          442         20                    0b           0b
//Int 104           14          284         20                    0b           0b
//Str 104           22          441         20                    0b           0b
//Int 108           14          281         20                    0b           0b
//Str 108           23          461         20                    0b           0b
//Int 116           14          281         20                    0b           0b
//Str 116           23          461         20                    0b           0b
//Int 132           14          283         20                    0b           0b
//Str 132           24          482         20                    0b           0b
//Int 164           14          280         20                    0b           0b
//Str 164           25          500         20                    0b           0b
//Int 228           14          280         20                    0b           0b
//Str 228           25          498         20                    0b           0b
//Int 356           14          281         20                    0b           0b
//Str 356           26          523         20                    0b           0b
//Int 612           14          283         20                    0b           0b
//Str 612           24          486         20                    0b           0b
//Int 1124          14          285         20                    0b           0b
//Str 1124          24          480         20                    0b           0b
//Int 2148          15          303         20                    0b           0b
//Str 2148          25          499         20                    0b           0b
//Int 4196          15          302         20                    0b           0b
//Str 4196          28          557         20                    0b           0b
//Int 8292          16          320         20                    0b           0b
//Str 8292          39          790         20                    0b           0b
//Int 16484         19          384         20                    0b           0b
//Str 16484         48          967         20                    0b           0b
//Int 32868         24          480         20                    0b           0b
//Str 32868         51          1009        20                    0b           0b
//Int 65636         27          537         20                    0b           0b
//Str 65636         50          1001        20                    0b           0b
//Int 131172        31          579         20                    0b           0b
//Str 131172        63          1292        20                    0b           0b
//Int 262244        31          634         20                    0b           0b
//Str 262244        108         2149        20                    0b           0b
//Int 524388        42          917         20                    0b           0b
//Str 524388        176         3547        20                    0b           0b
//Int 1048676       80          1745        20                    0b           0b
//Str 1048676       222         4508        20                    0b           0b
//Int 2097252       121         2450        20                    0b           0b
//Str 2097252       252         5053        20                    0b           0b
//Int 4194404       159         3242        20                    0b           0b
//Str 4194404       266         5325        20                    0b           0b
//Int 8388708       156         3136        20                    0b           0b
//Str 8388708       275         5568        20                    0b           0b
//Int 16777316      167         3334        20                    0b           0b
//Str 16777316      302         5965        20                    0b           0b
//Int 33554532      179         3475        20                    0b           0b
//Str 33554532      350         6655        20                    0b           0b
//Int 67108964      211         4169        20                    0b           0b
//Str 67108964      405         8025        20                    0b           0b
//Int 134217828     223         4475        20                    0b           0b
//Str 134217828     469         9204        20                    0b           0b


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
    public class DictionarySearchSpeedTest : TimeChecker
    {
        Random rnd = new Random();


        int[] dictIntKeys;
        Dictionary<int, int>[] dictInt = new Dictionary<int, int>[28];

        string[] dictStringKeys;
        Dictionary<string, string>[] dictString = new Dictionary<string, string>[28];

        public long a;
        public string b;

        const int cnt = 1000000;

        public DictionarySearchSpeedTest()
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
            for (int i = 0; i < dictInt.Length; i++)
            {
                var di = dictInt[i] = new Dictionary<int, int>();
                var ds = dictString[i] = new Dictionary<string, string>();
                for (int k = 0, kLen = (int)Math.Pow(2, i) + 100; k < kLen; k++)
                {
                    di[dictIntKeys[ k]] = dictIntKeys[k];
                    ds[dictStringKeys[k]] = dictStringKeys[k];
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
                        for (int j = 0; j < cnt; j++)
                        {
                            a += d[dictIntKeys[rnd.Next(0, d.Count)]];
                        }
                        this.a = a;
                    });
                    Proc("Str " + dictString[i].Count, 1, () =>
                    {
                        var d = dictString[i];
                        string b = null;
                        for (int j = 0; j < cnt; j++)
                        {
                            b = d[dictStringKeys[rnd.Next(0, d.Count)]];
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
