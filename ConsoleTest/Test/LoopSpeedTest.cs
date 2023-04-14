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
    public class LoopSpeedTest : TimeChecker
    {
        static int count = 100000;
        static Dictionary<string, Program> map = new Dictionary<string, Program>();
        static List<Program> list = new List<Program>();
        static Program[] array;
        static LinkedList<Program> linked = new LinkedList<Program>();
        static Program result;

        public LoopSpeedTest()
        {
            Console.WriteLine("Create");
            array = new Program[1000];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Program();
                list.Add(array[i]);
                map.Add(Guid.NewGuid().ToString(), array[i]);
                linked.AddLast(array[i]);
            }
            Console.WriteLine("Created");
            useBeginLog = true;

            Action _LoopArray = LoopArray;
            Action _LoopArrayNopt = LoopArrayNopt;
            Action _LoopArrayLocal = LoopArrayLocal;
            Action _LoopArrayLocalSet = LoopArrayLocalSet;
            Action _LoopArrayLocalSetEnumerable = LoopArrayLocalSetEnumerable;
            Action _LoopArrayLocalSetEnumerator = LoopArrayLocalSetEnumerator;
            Action _LoopList = LoopList;
            Action _LoopListLocal = LoopListLocal;
            Action _LoopListLocalSet = LoopListLocalSet;
            Action _LoopLinked = LoopLinked;
            Action _LoopMap = LoopMap;
            Action _LoopMapKey = LoopMapKey;
            Action _LoopMapValue = LoopMapValue;

            while (true)
            {
                Proc(count, _LoopArray);
                Proc(count, _LoopArrayNopt);
                Proc(count, _LoopArrayLocal);
                Proc(count, _LoopArrayLocalSet);
                //Proc(count, _LoopArrayLocalSetEnumerable);
                //Proc(count, _LoopArrayLocalSetEnumerator);
                Proc(count, _LoopList);
                Proc(count, _LoopListLocal);
                Proc(count, _LoopListLocalSet);
                Proc(count, _LoopLinked);
                Proc(count, _LoopMap);
                //Proc(count, _LoopMapKey);
                Proc(count, _LoopMapValue);

                Console.WriteLine(result.ToString());
                PrintSamples();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopArray()
        {
            Program item = null;
            for (int i = 0, iLen = array.Length; i < iLen; i++)
            {
                item = array[i];
            }
            result = item;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        void LoopArrayNopt()
        {
            Program item = null;
            for (int i = 0, iLen = array.Length; i < iLen; i++)
            {
                item = array[i];
            }
            result = item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopArrayLocal()
        {
            Program item = null;
            var array = LoopSpeedTest.array;
            for (int i = 0, iLen = array.Length; i < iLen; i++)
            {
                item = array[i];
            }
            result = item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopArrayLocalSet()
        {
            var array = LoopSpeedTest.array;
            for (int i = 0, iLen = array.Length; i < iLen; i++)
            {
                result = array[i];
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopArrayLocalSetEnumerable()
        {
            foreach (var e in E())
            {
                result = e;
            }
            IEnumerable<Program> E()
            {
                var array = LoopSpeedTest.array;
                for (int i = 0, iLen = array.Length; i < iLen; i++)
                {
                    yield return array[i];
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopArrayLocalSetEnumerator()
        {
            var e = E();
            while (e.MoveNext())
            {
                result = e.Current;
            }
            IEnumerator<Program> E()
            {
                var array = LoopSpeedTest.array;
                for (int i = 0, iLen = array.Length; i < iLen; i++)
                {
                    yield return array[i];
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopList()
        {
            Program item = null;
            for (int i = 0, iLen = list.Count; i < iLen; i++)
            {
                item = list[i];
            }
            result = item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopListLocal()
        {
            Program item = null;
            var list = LoopSpeedTest.list;
            for (int i = 0, iLen = list.Count; i < iLen; i++)
            {
                item = list[i];
            }
            result = item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopListLocalSet()
        {
            var list = LoopSpeedTest.list;
            for (int i = 0, iLen = list.Count; i < iLen; i++)
            {
                result = list[i];
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopMap()
        {
            Program item = null;
            foreach (var pair in LoopSpeedTest.map)
            {
                item = pair.Value;
            }
            result = item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopMapKey()
        {
            Program item = null;
            var map = LoopSpeedTest.map;
            foreach (var key in map.Keys)
            {
                item = map[key];
            }
            result = item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopMapValue()
        {
            Program item = null;
            var map = LoopSpeedTest.map;
            foreach (var key in map.Values)
            {
                item = key;
            }
            result = item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void LoopLinked()
        {
            Program item = null;
            var l = linked.First;
            do
            {
                item = l.Value;
            }
            while ((l = l.Next) != null);
            result = item;
        }
    }
}
