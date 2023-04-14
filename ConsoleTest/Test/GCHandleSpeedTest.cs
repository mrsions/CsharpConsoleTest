using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class GCHandleSpeedTest : TimeChecker
    {
        public struct CustomGCHandle
        {
            static List<CustomGCHandle> handles = new List<CustomGCHandle>(5000000) { default };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static CustomGCHandle FromIntPtr(int i)
            {
                return handles[i];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static CustomGCHandle Alloc(object o)
            {
                var v = new CustomGCHandle { id = handles.Count, Target = o };
                handles.Add(v);
                return v;
            }

            public int id;
            public object Target;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Free()
            {
                Target = null;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void FreeAll()
            {
                handles.Clear();
                handles.Add(default);
            }
        }
        public struct CustomGCHandle2
        {
            static object[] handles = new object[10000000];

            static int index = 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static object FromIntPtr(int i)
            {
                return handles[i];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Alloc(object o)
            {
                handles[++index] = o;
                return index;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void FreeAll()
            {
                for (int i = 0; i < handles.Length; i++)
                {
                    handles[i] = null;
                }
                index = 0;
            }
        }

        public GCHandleSpeedTest()
        {
            int length = 1000000;

            ABC[] abcs = new ABC[length];
            IntPtr[] ptrs = new IntPtr[length];
            int[] ptrs2 = new int[length];
            int[] ptrs3 = new int[length];

            while (true)
            {
                CustomGCHandle.FreeAll();
                CustomGCHandle2.FreeAll();

                GC.Collect(5, GCCollectionMode.Forced);
                BeginSample("Native New");
                for (int i = 0; i < length; i++)
                {
                    abcs[i] = new ABC();
                }
                EndSample();

                GC.Collect(5, GCCollectionMode.Forced);
                BeginSample("Ptr New");
                for (int i = 0; i < length; i++)
                {
                    var abc = new ABC();
                    ptrs[i] = (IntPtr)GCHandle.Alloc(abc);
                }
                EndSample();

                GC.Collect(5, GCCollectionMode.Forced);
                BeginSample("CPtr New");
                for (int i = 0; i < length; i++)
                {
                    var abc = new ABC();
                    ptrs2[i] = CustomGCHandle.Alloc(abc).id;
                }
                EndSample();

                GC.Collect(5, GCCollectionMode.Forced);
                BeginSample("CPtr2 New");
                for (int i = 0; i < length; i++)
                {
                    var abc = new ABC();
                    ptrs3[i] = CustomGCHandle2.Alloc(abc);
                }
                EndSample();

                int a = 0;

                BeginSample("Native GetAdd");
                for (int i = 0; i < length; i++)
                {
                    a += abcs[i].id;
                }
                EndSample();

                BeginSample("Ptr GetAdd");
                for (int i = 0; i < length; i++)
                {
                    var abc = GCHandle.FromIntPtr(ptrs[i]).Target as ABC;
                    a += abc.id;
                }
                EndSample();

                BeginSample("CPtr GetAdd");
                for (int i = 0; i < length; i++)
                {
                    var abc = CustomGCHandle.FromIntPtr(ptrs2[i]).Target as ABC;
                    a += abc.id;
                }
                EndSample();

                BeginSample("CPtr2 GetAdd");
                for (int i = 0; i < length; i++)
                {
                    var abc = CustomGCHandle2.FromIntPtr(ptrs3[i]) as ABC;
                    a += abc.id;
                }
                EndSample();

                BeginSample("Ptr Free");
                for (int i = 0; i < length; i++)
                {
                    GCHandle.FromIntPtr(ptrs[i]).Free();
                }
                EndSample();

                BeginSample("CPtr Free");
                for (int i = 0; i < length; i++)
                {
                    CustomGCHandle.FromIntPtr(ptrs2[i]).Free();
                }
                EndSample();

                PrintSamples();
            }
        }

        public class ABC
        {
            public int id;
        }
    }
}
