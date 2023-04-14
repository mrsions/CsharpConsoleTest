using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class SubstringSpeedTest : TimeChecker
    {
        public SubstringSpeedTest()
        {
            const int count = 10000000;
            string str = "aslkdfjaslkfasdfasasdfjaslkasjl";
            string strRst = "aslkdfjaslkfjaslkasjl";

            int[] array = str.ToCharArray().Select(v => (int)v).ToArray();
            int[] arrayRst;

            List<int> vv =new List<int>();

            int st = 10;
            int len = 20;

            while (true)
            {
                #region Substring
                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Substring(0, 5)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str.Substring(0, 5);
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Substring(0, len)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str.Substring(0, len);
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Substring(st, len)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str.Substring(st, len - st);
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Substring(st)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str.Substring(st);
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Substring(len)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str.Substring(len);
                };
                EndSample();
                #endregion

                #region SepOp
                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("SepOp(..5)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str[..5];
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("SepOp(..len)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str[..len];
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("SepOp(st..len)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str[st..len];
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("SepOp(st..)");
                for (int i = 0; i < count; i++)
                {
                    strRst = str[st..];
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("SepOp(len..]");
                for (int i = 0; i < count; i++)
                {
                    strRst = str[len..];
                };
                EndSample();
                #endregion





                #region ArraySplit
                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Subarraying(0, 5)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = SplitArray(array, 0, 5);
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Subarraying(0, len)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = SplitArray(array, 0, len);
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Subarraying(st, len)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = SplitArray(array, st, len - st);
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Subarraying(st)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = SplitArray(array, st);
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("Subarraying(len)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = SplitArray(array, len);
                };
                EndSample();
                #endregion

                #region ArraySepOp
                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("ArraySepOp(..5)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = array[..5];
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("ArraySepOp(..len)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = array[..len];
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("ArraySepOp(st..len)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = array[st..len];
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("ArraySepOp(st..)");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = array[st..];
                };
                EndSample();

                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                BeginSample("ArraySepOp(len..]");
                for (int i = 0; i < count; i++)
                {
                    arrayRst = array[len..];
                };
                EndSample();
                #endregion

                PrintSamples();
            }
        }

        private int[] SplitArray(int[] array, int start)
        {
            int len = array.Length - start;
            int[] rst = new int[len];
            Buffer.BlockCopy(array, start, rst, 0, len * sizeof(int));
            return rst;
        }

        private int[] SplitArray(int[] array, int start, int len)
        {
            int[] rst = new int[len];
            Buffer.BlockCopy(array, start, rst, 0, len * sizeof(int));
            return rst;
        }
    }
}
