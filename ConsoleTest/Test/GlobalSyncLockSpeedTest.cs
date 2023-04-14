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
    public class GlobalSyncLockSpeedTest : TimeChecker
    {
        int count = 10000000;

        static int a = 0;
        static object o = new object();

        public GlobalSyncLockSpeedTest()
        {
            while (true)
            {
                Proc(count, StaticSyncMethod);
                Proc(count, StaticSyncStaticObject);
                Proc(count, StaticSyncTypeOf);
                Proc(count, SyncStaticObject);
                Proc(count, SyncStaticOtherObject);
                Proc(count, SyncGetType);
                Proc(count, SyncTypeOf);
                PrintSamples();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized | MethodImplOptions.NoInlining)]
        static void StaticSyncMethod()
        {
            a++;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void StaticSyncStaticObject()
        {
            lock (o)
            {
                a++;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void StaticSyncTypeOf()
        {
            lock (typeof(GlobalSyncLockSpeedTest))
            {
                a++;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void SyncStaticObject()
        {
            lock (o)
            {
                a++;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void SyncStaticOtherObject()
        {
            lock (Singleton<Program>.main)
            {
                a++;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void SyncGetType()
        {
            lock (GetType())
            {
                a++;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void SyncTypeOf()
        {
            lock (typeof(GlobalSyncLockSpeedTest))
            {
                a++;
            }
        }
    }
}
