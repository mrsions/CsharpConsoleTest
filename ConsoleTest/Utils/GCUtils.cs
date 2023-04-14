#define Logging2

using System;
using System.Diagnostics;
using System.Threading;

namespace Realwith.NetPack
{
    public static class GCUtils
    {
        static int[] gen = new int[3];

        public static void WriteGen()
        {
            gen[0] = GC.CollectionCount(0);
            gen[1] = GC.CollectionCount(1);
            gen[2] = GC.CollectionCount(2);
        }

        public static (int g1, int g2, int g3) GetGen()
        {
            return (GC.CollectionCount(0) - gen[0],
                    GC.CollectionCount(1) - gen[1],
                    GC.CollectionCount(2) - gen[2]);
        }

        public static void FullGC()
        {
            GC.Collect(2);
            GC.WaitForFullGCComplete();
        }
    }
}
