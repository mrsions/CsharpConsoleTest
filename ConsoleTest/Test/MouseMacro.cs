using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ConsoleTest.Test
{
    public class MouseMacro
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32")]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);
        public static string GetClassName(IntPtr hwnd) { var sb = new StringBuilder(0xFF); GetClassName(hwnd, sb, sb.Capacity); return sb.ToString(); }

        [DllImport("user32")]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);
        public static string GetWindowText(IntPtr hwnd) { var sb = new StringBuilder(0xFF); GetWindowText(hwnd, sb, sb.Capacity); return sb.ToString(); }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        IntPtr MakeLParam(int x, int y) => (IntPtr)((y << 16) | (x & 0xFFFF));

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_RBUTTONUP = 0x0205;
        const int WM_MOUSEMOVE = 0x0200;

        void PerformLefttClick(IntPtr hwnd, int x, int y)
        {
            Console.WriteLine("    -> LeftClick " + x + "x" + y);
            var point = new Point(x, y);
            var pointPtr = MakeLParam(point.x, point.y);
            SendMessage(hwnd, WM_MOUSEMOVE, IntPtr.Zero, pointPtr);
            Thread.Sleep(10);
            SendMessage(hwnd, WM_LBUTTONDOWN, IntPtr.Zero, pointPtr);
            Thread.Sleep(10);
            SendMessage(hwnd, WM_LBUTTONUP, IntPtr.Zero, pointPtr);
            Thread.Sleep(10);
        }
        void PerformRightClick(IntPtr hwnd, int x, int y)
        {
            Console.WriteLine("    -> RightClick " + x + "x" + y);
            var point = new Point(x, y);
            var pointPtr = MakeLParam(point.x, point.y);
            SendMessage(hwnd, WM_MOUSEMOVE, IntPtr.Zero, pointPtr);
            Thread.Sleep(10);
            SendMessage(hwnd, WM_RBUTTONDOWN, IntPtr.Zero, pointPtr);
            Thread.Sleep(10);
            SendMessage(hwnd, WM_RBUTTONUP, IntPtr.Zero, pointPtr);
            Thread.Sleep(10);
        }
        public MouseMacro()
        {
            while (true)
            {
                Point p = new Point(145, 120);

                var processes = Process.GetProcessesByName("D2R");
                foreach (var proc in processes)
                {
                    if (proc.MainWindowHandle != IntPtr.Zero)
                    {
                        PrintPtr(proc.MainWindowHandle);
                        PrintPtr(FindWindowEx(proc.MainWindowHandle, IntPtr.Zero, null, null));
                        var childs = new WindowHandleInfo(proc.MainWindowHandle).GetAllChildHandles();
                        foreach (var c in childs)
                        {
                            PrintPtr(c);
                        }

                        var ptr = proc.MainWindowHandle;
                        PrintPtr(ptr);

                        GetWindowRect(ptr, out var rect);
                        var w = rect.Right - rect.Left;
                        var h = rect.Bottom - rect.Top;
                        Console.WriteLine("    -> " + w + "x" + h);

                        PerformLefttClick(ptr, 100, 60);
                        //PerformLefttClick(ptr, 1900, 1040);

                        //PerformLefttClick(ptr, rect.Left + (int)(w * 0.3f), rect.Top + (int)(h * 0.3f));
                        //PerformRightClick(ptr, rect.Left + (int)(w * 0.3f), rect.Top + (int)(h * 0.3f));
                        //PerformLefttClick(ptr, rect.Left + (int)(w * 0.3f), rect.Top + (int)(h * 0.3f));
                        //PerformRightClick(ptr, rect.Left + (int)(w * 0.3f), rect.Top + (int)(h * 0.3f));
                    }
                }

                Console.ReadLine();
            }
        }

        private static void PrintPtr(IntPtr ptr)
        {
            Console.WriteLine(ptr + " - " + GetClassName(ptr) + " / " + GetWindowText(ptr));
        }
    }

    public class WindowHandleInfo
    {
        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        private IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)
        {
            this._MainHandle = handle;
        }

        public List<IntPtr> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                WindowEnumProc childProc = new WindowEnumProc(EnumWindow);
                EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        private int EnumWindow(IntPtr hwnd, IntPtr lparam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lparam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return 0;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hwnd);

            return 1;
        }
    }
}
