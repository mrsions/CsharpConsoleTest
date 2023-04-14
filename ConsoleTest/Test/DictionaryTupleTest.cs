using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleTest
{
    public class DictionaryTupleTest
    {
        public class TempClass
        {
        }

        static TempClass b;
        static TempClass c;
        static TempClass d;
        static TempClass e;
        static TempClass f;

        public static void Run()
        {
            var a = new Dictionary<(TempClass, TempClass), float>();

            b = new TempClass();
            c = new TempClass();
            d = new TempClass();
            e = new TempClass();
            f = new TempClass();

            a.Add((b, c), 1);
            a.Add((c, d), 2);
            a.Add((d, e), 3);
            a.Add((e, f), 4);
            a.Add((c, b), 1);
            a.Add((d, c), 2);
            a.Add((e, d), 3);
            a.Add((f, e), 4);

            new Thread(Runn).Start(a);
        }

        static void Runn(object _a)
        {
            var a = (Dictionary<(TempClass, TempClass), float>)_a;

            if (a[(b, c)] == 1) System.Console.WriteLine("Ok"); else System.Console.WriteLine("No");
            if (a[(c, d)] == 2) System.Console.WriteLine("Ok"); else System.Console.WriteLine("No");
            if (a[(d, e)] == 3) System.Console.WriteLine("Ok"); else System.Console.WriteLine("No");
            if (a[(e, f)] == 4) System.Console.WriteLine("Ok"); else System.Console.WriteLine("No");
            if (a[(c, b)] == 1) System.Console.WriteLine("Ok"); else System.Console.WriteLine("No");
            if (a[(d, c)] == 2) System.Console.WriteLine("Ok"); else System.Console.WriteLine("No");
            if (a[(e, d)] == 3) System.Console.WriteLine("Ok"); else System.Console.WriteLine("No");
            if (a[(f, e)] == 4) System.Console.WriteLine("Ok"); else System.Console.WriteLine("No");
        }

    }
}
