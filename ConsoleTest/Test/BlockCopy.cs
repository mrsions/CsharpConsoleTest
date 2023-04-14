using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleTest.Test
{
    public class BlockCopy
    {
        public struct Vector4
        {
            public float x;
            public float y;
            public float z;
            public float w;

            public Vector4(float oo) : this()
            {
                x = oo;
                y = oo;
                z = oo;
                w = oo;
            }
        }

        public static void Run()
        {
            //decimal[][] dm = new decimal[][]
            //{
            //    new decimal[10000],
            //    new decimal[10000],
            //    new decimal[10000],
            //    new decimal[10000],
            //    new decimal[10000]
            //};
            //decimal oo = 0;
            //decimal[] dm2 = null, dm3 = null, dm4 = null;

            //for (int i = 0; i < dm.Length; i++)
            //{
            //    for (int j = 0; j < dm[i].Length; j++)
            //    {
            //        oo += 1m;
            //        dm[i][j] = oo;
            //    }
            //}

            Vector4[][] dm = new Vector4[][]
            {
                new Vector4[60000],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333],
                //new Vector4[3333]
            };
            float oo = 0;
            Vector4[] dm2 = null, dm3 = null, dm4 = null;

            for (int i = 0; i < dm.Length; i++)
            {
                for (int j = 0; j < dm[i].Length; j++)
                {
                    oo += 1f;
                    dm[i][j] = new Vector4(oo);
                }
            }

            var st = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                dm2 = BlockMerge(dm);
            }
            st.Stop();
            //var st2 = Stopwatch.StartNew();
            //for (int i = 0; i < 10000; i++)
            //{
            //    dm3 = UnsafeBlockMerge(dm);
            //}
            //st2.Stop();
            var st3 = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                dm4 = UnsafeBlockMerge2(dm);
            }
            st3.Stop();

            Console.WriteLine(st.ElapsedMilliseconds);
            //Console.WriteLine(st2.ElapsedMilliseconds);
            Console.WriteLine(st3.ElapsedMilliseconds);

            var r1 = (from a in dm from b in a select b.x).Sum();
            var r2 = (from a in dm2 select a.x).Sum();
            //var r3 = (from a in dm3 select a.x).Sum();
            var r4 = (from a in dm4 select a.x).Sum();
            //Console.WriteLine(r1 == r2 && r2 == r3 && r3 == r4);
            Console.WriteLine(r1 == r2 && r2 == r4);
        }

        private static T[] BlockMerge<T>(IEnumerable<T[]> em)
            where T : struct
        {
            var list = em.ToList();

            int len = list.Sum(v => v.Length);
            T[] rst = new T[len];

            int offset = 0;
            for (int i = 0; i < list.Count; i++)
            {
                T[] tm = list[i];
                Array.Copy(tm, 0, rst, offset, tm.Length);
                offset += tm.Length;
            }

            return rst;
        }

        private static unsafe decimal[] UnsafeBlockMerge(IEnumerable<decimal[]> em)
        {
            var list = em.ToList();

            int len = list.Sum(v => v.Length);
            decimal[] dstArr = new decimal[len];
            fixed (decimal* dstPtr = &dstArr[0])
            {
                byte* vDstPtr = (byte*)dstPtr;
                int sizeT = Marshal.SizeOf<decimal>();
                int availableDst = len * sizeT;

                for (int i = 0; i < list.Count; i++)
                {
                    decimal[] srcArr = list[i];
                    int voidLength = srcArr.Length * sizeT;

                    fixed (decimal* srcPtr = &srcArr[0])
                    {
                        byte* vSrcPtr = (byte*)srcPtr;
                        Buffer.MemoryCopy(vSrcPtr, vDstPtr, availableDst, voidLength);
                    }

                    vDstPtr += voidLength;
                    availableDst -= voidLength;
                }
            }
            return dstArr;
        }

        private static unsafe T[] UnsafeBlockMerge2<T>(IEnumerable<T[]> em)
        {
            //List<T> list = em.ToList();
            int sizeT = Marshal.SizeOf<decimal>();

            int len = em.Sum(v => v.Length);
            int availableDst = len * sizeT;

            T[] dstArr = new T[len];
            GCHandle dstHdl = GCHandle.Alloc(dstArr, GCHandleType.Pinned);
            byte* dstPtr = (byte*)dstHdl.AddrOfPinnedObject();
            {
                foreach (var srcArr in em)
                {
                    int voidLength = srcArr.Length * sizeT;

                    GCHandle srcHdl = GCHandle.Alloc(srcArr, GCHandleType.Pinned);
                    byte* srcPtr = (byte*)srcHdl.AddrOfPinnedObject();
                    {
                        Buffer.MemoryCopy(srcPtr, dstPtr, availableDst, voidLength);
                    }
                    srcHdl.Free();

                    dstPtr += voidLength;
                    availableDst -= voidLength;
                }
            }
            dstHdl.Free();
            return dstArr;
        }
    }
}
