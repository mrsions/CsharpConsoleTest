using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MonoConsoleTest
{
    public unsafe class Program
    {
        public static void Main()
        {
            string v = "asdfasdfasdfasdfs";
            int len = 10;

            A(v, len);
            B(v, len);
        }

        static string A(string s, int len)
        {
            return s.Substring(0, len);
        }

        static string B(string s, int len)
        {
            return s[..len];
        }
    }
}
