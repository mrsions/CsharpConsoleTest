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
    public class DictionaryInputSpeedTest : TimeChecker
    {
        static new Dictionary<long, long> dict = new Dictionary<long, long>();
        static Random rnd = new Random();

        static long NewID()
        {
            return (long)rnd.Next(int.MinValue, int.MaxValue) * rnd.Next(int.MinValue, int.MaxValue);
        }

        public DictionaryInputSpeedTest()
        {

            int cnt = 100;

            while (true)
            {
                dict.Clear();
                ForceGC();
                Proc("Test", cnt, () =>
                 {
                     var dict = DictionaryInputSpeedTest.dict;
                     for (int i = 0; i < 100000; i++)
                     {
                         long id = DictionaryInputSpeedTest.NewID();
                         dict[id] = id;
                     }
                 });

                dict.Clear();
                ForceGC();
                Proc("Test2", cnt, () =>
                {
                     var dict = DictionaryInputSpeedTest.dict;
                    for (int i = 0; i < 100000; i++)
                    {
                         long id = DictionaryInputSpeedTest.NewID();
                        if (!dict.ContainsKey(id))
                        {
                            dict.Add(id, id);
                        }
                    }
                });

                dict.Clear();
                ForceGC();
                Proc("Test3", cnt, () =>
                {
                     var dict = DictionaryInputSpeedTest.dict;
                    for (int i = 0; i < 100000; i++)
                    {
                         long id = DictionaryInputSpeedTest.NewID();
                        if (!dict.ContainsKey(id))
                        {
                            dict[id] = id;
                        }
                    }
                });

                dict.Clear();
                ForceGC();
                Proc("Test4", cnt, () =>
                {
                     var dict = DictionaryInputSpeedTest.dict;
                    for (int i = 0; i < 100000; i++)
                    {
                         long id = DictionaryInputSpeedTest.NewID();
                        try
                        {
                            dict.Add(id, id);
                        }
                        catch { }
                    }
                });

                PrintSamples();
            }
        }
    }
}
