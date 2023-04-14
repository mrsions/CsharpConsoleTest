using Microsoft.ClearScript.V8;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ConsoleTest.Test
{

    public class ClearScriptTest : TimeChecker
    {

        public ClearScriptTest()
        {
            V8ScriptEngine engine = new V8ScriptEngine();
            int count = 1000000;

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["active"] = 5;
            engine.AddHostObject("dict", Microsoft.ClearScript.HostItemFlags.GlobalMembers, dict);

            ITestv rc2 = CSharpCpompiler.Compile<ITestv>("int rst = 0; for(int i=0; i<1000; i++) if (data.active == 5) { rst++; } else { rst++; } return rst > 0;");
            Testv native = new Testv();
            MethodInfo rc1 = CSharpCpompiler.Compile("int rst = 0; for(int i=0; i<1000; i++) if (data.active == 5) { rst++; } else { rst++; } return rst > 0;", new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("data", new Data()) });
            var rc1d = (Func<Data, bool>)rc1.CreateDelegate(typeof(Func<Data, bool>));
            engine.Evaluate("function ABCDEFGHIJKLMNOPQASLKDFJASJLESKAFJ(active) { var rst = 0; for(var i=0; i<10; i++) if (active == 5) { rst++; } else { rst++; } return rst > 0; }");
            Data data = new Data();
            data.active = 5;


            while (true)
            {
                BeginSample("C# Runtime Interface");
                {
                    int v = 0;
                    for (int i = 0; i < count; i++)
                    {
                        data.active = i;
                        if (rc2.Main(data))
                        {
                            v++;
                        }
                    }
                    Debug.Assert(v == 1);
                }
                EndSample();

                BeginSample("C# Native");
                {
                    int v = 0;
                    for (int i = 0; i < count; i++)
                    {
                        data.active = i;
                        if (native.Main(data))
                        {
                            v++;
                        }
                    }
                    Debug.Assert(v == 1);
                }
                EndSample();

                BeginSample("C# Runtime");
                {
                    int v = 0;
                    for (int i = 0; i < count; i++)
                    {
                        data.active = i;
                        if (rc1d(data))
                        {
                            v++;
                        }
                    }
                    Debug.Assert(v == 1);
                }
                EndSample();

                PrintSamples();
            }
        }

        public interface ITestv
        {
            bool Main(Data data);
        }
        public class Testv
        {
            public bool Main(Data data)
            {
                int rst = 0; for (int i = 0; i < 1000; i++) if (data.active == 5) { rst++; } else { rst++; }
                return rst > 0;
            }
        }

        public class Data
        {
            public int active;
        }
    }
}
