using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using SFramework;

namespace ConsoleTest.Test
{
    public class BigGuidTest
    {
        public BigGuidTest()
        {
            int j = 0;
            while (true)
            {
                //byte[] data = new byte[65];
                //new Random().NextBytes(data);

                //BigGuid v = new BigGuid(data);
                //BigGuid v2 = new BigGuid(v.ToString());

                Console.WriteLine(BigGuid.NewGuid());

            }
        }
    }
}
