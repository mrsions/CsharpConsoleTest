using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleTest.Test
{
    public class NumberOfCase
    {
        //public static Run()
        //{
        //    string[] temps = new string[]
        //    {
        //        "x",
        //        "y",
        //        "z",
        //    };
        //    string join = " * ";
        //    string prefix = "";
        //    string subfix = "";

        //    for (int i = 0; i < temps.Length; i++)
        //    {
        //        for (int j = 0; j < temps.Length; j++)
        //        {
        //            if (j == i) continue;
        //            for (int k = 0; k < temps.Length; k++)
        //            {
        //                if (j == k || i == k) continue;

        //                for (int w = 0; w < temps.Length; w++)
        //                {
        //                    if (j == w || i == w || k == w) continue;

        //                    Console.WriteLine(prefix + temps[i] + join + temps[j] + join + temps[k] + join + temps[w] + subfix);
        //                }
        //            }
        //        }
        //    }


        //    Console.WriteLine("---------------------------------");
        //    var items = temps.Select(v => new Item { item = v }).ToArray();

        //    while (Increase(items))
        //    {
        //        if (Validate(items))
        //        {
        //            var r = Result(items);
        //            StringBuilder sb = new StringBuilder();
        //            sb.Append(prefix);
        //            for (int i = 0; i < r.Length; i++)
        //            {
        //                if (i != 0) sb.Append(join);
        //                sb.Append(r[i].item);
        //            }
        //            sb.Append(subfix);

        //            Console.WriteLine(sb);
        //        }
        //    }
        //}

        public abstract class BaseItem
        {
            public int index = 0;
        }

        public class StringItem : BaseItem
        {
            public string item;

            public StringItem(string item)
            {
                this.item = item;
            }

            public override string ToString()
            {
                return item;
            }
        }

        public class ItemArray : BaseItem
        {
            public BaseItem[] items;

            public BaseItem this[int index] => items[index];

            public int Length => items.Length;

            public ItemArray(BaseItem[] items)
            {
                this.items = items;
            }

            public void ClearIndex()
            {
                for(int i=0; i<items.Length; i++)
                {
                    items[i].index = 0;
                }
            }
        }

        //public static IEnumerable<BaseItem[]> Calculate(ItemArray items)
        //{
        //    items.ClearIndex();
        //    while (Increase(items))
        //    {
        //        if (Validate(items))
        //        {
        //            var rst = Result(items);

        //            bool isResulted = false;
        //            for(int i=0; i<rst.Length; i++)
        //            {
        //                if(rst[i] is ItemArray)
        //                {
        //                    isResulted = true;
        //                }
        //            }
        //        }
        //    }
        //}

        private static bool Increase(ItemArray items)
        {
            int len = items.Length;
            int i = len - 1;

            while (i >= 0)
            {
                if (items[i].index + 1 == len)
                {
                    items[i].index = 0;
                    if (--i < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    break;
                }
            }

            items[i].index++;
            return true;
        }

        private static bool Validate(ItemArray items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = i + 1; j < items.Length; j++)
                {
                    if (items[i].index == items[j].index)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static BaseItem[] Result(ItemArray items)
        {
            BaseItem[] rst = new BaseItem[items.Length];
            Result(items, rst);
            return rst;
        }

        private static void Result(ItemArray items, BaseItem[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[items.items[i].index] = items.items[i];
            }
        }
    }
}
