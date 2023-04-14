using Microsoft.ClearScript.V8;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
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
using System.Runtime.Loader;
using System.Text;
using System.Threading;

namespace ConsoleTest.Test
{
    public class CSharpCpompiler : TimeChecker
    {
        public static MethodInfo Compile(string code, params KeyValuePair<string, object>[] values)
        {
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.Add(typeof(object).Assembly);

            StringBuilder paramNames = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in values)
            {
                if (paramNames.Length != 0) paramNames.Append(", ");
                AddParam(assemblies, pair.Key, pair.Value.GetType(), paramNames);
            }

            code = $@"using System;
public class Program
{{
    public static bool Main({paramNames})
    {{
        {code}
    }}
}}";

            var assembly = MakeAssembly(code, assemblies) ?? throw new NullReferenceException("Not found assembly");
            var type = assembly.GetType("Program") ?? throw new NullReferenceException("Not found Program class");
            return type.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
        }

        public static T Compile<T>(string code)
        {
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.Add(typeof(object).Assembly);
            assemblies.Add(typeof(T).Assembly);

            var t = typeof(T);
            var methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (methods.Length != 1) throw new ArgumentException("Required have only 1 method.");

            var method = methods.First();

            StringBuilder paramNames = new StringBuilder();
            foreach (var param in method.GetParameters())
            {
                if (paramNames.Length != 0) paramNames.Append(", ");
                AddParam(assemblies, param.Name, param.ParameterType, paramNames);
            }

            code = $@"using System;

public class Program : {typeof(T).FullName.Replace('+', '.')}
{{
    public {method.ReturnType.FullName.Replace('+', '.')} {method.Name}({paramNames})
    {{
        {code}
    }}
}}";

            var assembly = MakeAssembly(code, assemblies) ?? throw new NullReferenceException("Not found assembly");
            var type = assembly.GetType("Program") ?? throw new NullReferenceException("Not found Program class");
            return (T)Activator.CreateInstance(type);
        }

        private static Assembly MakeAssembly(string code, List<Assembly> assemblies)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            MetadataReference[] references = assemblies.Select(r =>
            {
                string codeBase = r.GetName().CodeBase;
                if (codeBase.StartsWith("file:///", StringComparison.OrdinalIgnoreCase))
                {
                    codeBase = codeBase.Substring("file:///".Length);
                }
                return MetadataReference.CreateFromFile(codeBase);
            }).ToArray();

            CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release);

            CSharpCompilation compilation = CSharpCompilation.Create(
                $"Custom{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: options);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    return AssemblyLoadContext.Default.LoadFromStream(ms);
                }
                else
                {
                    foreach (var r in result.Diagnostics)
                    {
                        Console.WriteLine(r);
                    }
                }
                return null;
            }
        }

        private static void AddParam(List<Assembly> assemblies, string name, Type valueType, StringBuilder sb = null)
        {
            assemblies.AddIfHaveNot(valueType.Assembly);
            if (sb != null)
            {
                sb.Append(valueType.FullName.Replace("+", ".")).Append(' ').Append(name);
            }
        }
    }
}
