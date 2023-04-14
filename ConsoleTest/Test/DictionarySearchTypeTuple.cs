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
    public class DictionarySearchTypeTuple : TimeChecker
    {
        public DictionarySearchTypeTuple()
        {
            var a = new Dictionary<(Type, Type), string>();
            var b = new Dictionary<Type, Dictionary<Type, string>>();

            var types = typeof(string).Assembly.GetTypes();
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    a[(types[i], types[j])] = types[i].FullName + " to " + types[j].FullName;

                    if (!b.TryGetValue(types[i], out var list))
                    {
                        b[types[i]] = list = new Dictionary<Type, string>();
                    }
                    list[types[j]] = types[i].FullName + " to " + types[j].FullName;
                }
            }

                int cnt = 1000;

            while (true)
            {
                ForceGC();
                Proc("Tuple", cnt, () =>
                 {
                     string type;
                     for (int i = 0; i < 100; i++)
                     {
                         for (int j = 0; j < 100; j++)
                         {
                             type = a[(types[i], types[j])];
                         }
                     }
                 });

                ForceGC();
                Proc("Double", cnt, () =>
                 {
                     string type;
                     for (int i = 0; i < 100; i++)
                     {
                         for (int j = 0; j < 100; j++)
                         {
                             if (b.TryGetValue(types[i], out var list))
                             {
                                 type = list[types[j]];
                             }
                         }
                     }
                 });

                PrintSamples();
            }
        }
    }
}
