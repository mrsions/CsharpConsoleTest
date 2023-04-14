using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleTest
{
    public class HashSetTest
    {
        public static void Run()
        {
            IList<int> rst = new List<int> { 1,20 };
            HashSet<int>[] variants = new HashSet<int>[]{
                new HashSet<int> { 10, 11, 12, 13, 14, 15 },
                new HashSet<int> { 10, 11, 13, 15, 16, 17 }
            };

            var current = new HashSet<int>(rst);
            for (int i = 0; i < variants.Length && current.Count > 1; i++)
            {
                var next = variants[i];
                next.IntersectWith(current);
                if (next.Count > 0)
                {
                    current = next;
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine(current.FirstOrDefault());
        }
    }
}
