using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest.Test
{
    class ForForeach
    {
        public static int Calculate(int start, int end) => start * end + (start / end) + (start % 5) + (end % 3);


        public static void Test()
        {

            int[] array = new int[100];
            List<int> list = new List<int>();

            for (int i = 0; i < 100; i++) { }
            for (int i = 0; i < array.Length; i++) { }
            for (int i = 0, len = list.Count; i < len; i++) { }

            foreach (var value in array) { }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var pair in dictionary) { }

            HashSet<string> hashSet = new HashSet<string>();
            foreach (var value in hashSet) { }

        }

        public int SomeMethod(int a, int b)
        {
            return sum(a, b);

            int sum(int v1, int v2)
            {
                return v1 + v2;
            }
        }
    }
}
