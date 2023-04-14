using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class HashSetLoopSpeed : TimeChecker
    {
        public HashSetLoopSpeed()
        {
            const int count = 100000000;

            var array = Enumerable.Range(0, count).ToArray();
            var list = array.ToList();
            var hash = array.ToHashSet();

            IReadOnlyCollection<int> array2 = new ReadOnlyCollection<int>(array);
            IReadOnlyCollection<int> list2 = new ReadOnlyCollection<int>(list);
            IReadOnlyCollection<int> hash2 = new ReadonlyHashSet<int>(hash);

            int rst = 0;

            while (true)
            {
                BeginSample("Array Loop");
                for (int i = 0, iLen = array.Length; i < iLen; i++)
                {
                    rst += array[i];
                };
                EndSample();

                BeginSample("Array Foreach");
                foreach (var item in array)
                {
                    rst += item;
                };
                EndSample();

                BeginSample("List Loop");
                for (int i = 0, iLen = list.Count; i < iLen; i++)
                {
                    rst += array[i];
                };
                EndSample();

                BeginSample("List Foreach");
                foreach (var item in list)
                {
                    rst += item;
                };
                EndSample();

                BeginSample("HashSet Foreach");
                foreach (var item in hash)
                {
                    rst += item;
                };
                EndSample();

                BeginSample("Readonly Array Foreach");
                foreach (var item in array2)
                {
                    rst += item;
                };
                EndSample();

                BeginSample("Readonly List Foreach");
                foreach (var item in list2)
                {
                    rst += item;
                };
                EndSample();

                BeginSample("Readonly HashSet Foreach");
                foreach (var item in hash2)
                {
                    rst += item;
                };
                EndSample();

                PrintSamples();
            }
        }

        public class ReadonlyHashSet<T> : IReadOnlyCollection<T>
        {
            public int Count => set.Count;
            private HashSet<T> set;

            public ReadonlyHashSet(HashSet<T> set) => this.set = set;

            public bool Contains(T i) => set.Contains(i);

            public HashSet<T>.Enumerator GetEnumerator() => set.GetEnumerator();

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => set.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => set.GetEnumerator();
        }
    }
}
