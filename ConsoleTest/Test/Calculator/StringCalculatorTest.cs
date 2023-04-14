using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;

namespace ConsoleTest
{
    public class StringCalculatorTest
    {

        public static void Run()
        {
            Profiling();

            while (true)
            {
                string line = Console.ReadLine();
                PrintCalc(line);
                //PrintCalc("(12+32)+32");
                //PrintCalc("(12+32)+(12+32)");
                //PrintCalc("(12+32)+(12+(12+(32+32+52)))+(32+32)");
            }
        }

        private static void Profiling()
        {
            string logic = "(pi^1.53)+52/1.2-12*((({0}+{1}*5)))";

            int loop = 1000000;

            {
                double a = 0;
                double id = 0;
                Stopwatch st = Stopwatch.StartNew();
                for (int i = 0; i < loop; i++)
                {
                    a += Math.Pow(Math.PI, 1.53) + 52 / 1.2 - 12 * ((((id += 0.0001d) + (id) * 5)));
                }
                st.Stop();
                Console.WriteLine("Profiling A " + (int)st.ElapsedMilliseconds + " / " + a);
            }// 5.7628764016596079516821221351652 + 43.333333333333333333333333333333 - 0.0012
            {// 5.7628764016596081		          + 43.333333333333336                - 0.0072


                double a = 0;
                double id = 0;
                var calculator = new FormatCalculator();
                calculator.AddConstant("addId", () => id += 0.0001);
                calculator.AddConstant("ID", () => id);

                Stopwatch st = Stopwatch.StartNew();
                calculator.Setup(string.Format(logic, "addid", "id"));
                for (int i = 0; i < loop; i++)
                {
                    a += calculator.Calculate();
                }
                st.Stop();
                Console.WriteLine("Profiling B " + (int)st.ElapsedMilliseconds + " / " + a);
            }
        }

        static int id = 0;
        private static void PrintCalc(string v)
        {
            try
            {
                var calculator = new FormatCalculator();
                calculator.Setup(v);
                Console.WriteLine(" = " + calculator.Calculate());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }

    public class BaseCalculator
    {

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    DEFINE
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 1+1 , 12+5, 52*45
        /// 등의 숫자를 골라낸다
        /// </summary>
        protected static Regex FindConstant = new Regex(@"\b[a-zA-Z][\w_]*\b(?![\(\[])");

        /// <summary>
        /// 1+1 , 12+5, 52*45, 1<<1 1>>1 1&1 4|4
        /// 등의 숫자를 골라낸다
        /// </summary>
        protected static Regex DetectCalculate = new Regex($@"([\d\.]+)\s*([" + @"&|" + @"<>" + @"\^" + @"\*\/+" + @"\+\-+" + @"]+)\s*([\d\.]+)");
        protected static Regex[] DetectCalculates = new[]
        {
            new Regex($@"([\d\.]+)\s*([" + @"&|"    + @"]+)\s*([\d\.]+)"),
            new Regex($@"([\d\.]+)\s*([" + @"<>"    + @"]+)\s*([\d\.]+)"),
            new Regex($@"([\d\.]+)\s*([" + @"\^"    + @"]+)\s*([\d\.]+)"),
            new Regex($@"([\d\.]+)\s*([" + @"\*\/+" + @"])\s*([\d\.]+)"),
            new Regex($@"([\d\.]+)\s*([" + @"\+\-+" + @"])\s*([\d\.]+)")
        };

        /// <summary>
        /// mod(1,2)
        /// 등의 함수를 찾아낸다.
        /// </summary>
        protected static Regex FindFunc2 = new Regex(@"(\w+)\s*\(\s*([\d\.]+)\s*,\s*([\d\.]+)\s*\)");

        /// <summary>
        /// 괄호에 쌓여있는 숫자를 골라낸다.
        /// (3)   (2)   (322.5) 
        /// </summary>
        protected static Regex FindFunction1 = new Regex(@"(\w*)\s*\(\s*([\d\.]+)\s*\)\s*");

        /// <summary>
        /// 입력된 글자에서 숫자만 찾아낸다
        /// </summary>
        protected static Regex OnlyNumber = new Regex(@"[\d\.]+");


        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    STATIC
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        // 상수
        protected static Dictionary<string, Func<double>> GlobalConstants = new Dictionary<string, Func<double>>();

        // 2항 함수
        protected static Dictionary<string, Func<double, double, double>> GlobalFunctions2 = new Dictionary<string, Func<double, double, double>>();

        // 1항 함수
        protected static Dictionary<string, Func<double, double>> GlobalFunctions1 = new Dictionary<string, Func<double, double>>();

        static BaseCalculator()
        {
            foreach (var m in typeof(Math).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                double v;
                if (m.FieldType == typeof(double)) v = (double)m.GetValue(null);
                else if (m.FieldType == typeof(float)) v = (double)(float)m.GetValue(null);
                else if (m.FieldType == typeof(int)) v = (double)(int)m.GetValue(null);
                else if (m.FieldType == typeof(long)) v = (double)(long)m.GetValue(null);
                else if (m.FieldType == typeof(short)) v = (double)(short)m.GetValue(null);
                else if (m.FieldType == typeof(byte)) v = (double)(byte)m.GetValue(null);
                else if (m.FieldType == typeof(sbyte)) v = (double)(sbyte)m.GetValue(null);
                else if (m.FieldType == typeof(uint)) v = (double)(uint)m.GetValue(null);
                else if (m.FieldType == typeof(ulong)) v = (double)(ulong)m.GetValue(null);
                else if (m.FieldType == typeof(ushort)) v = (double)(ushort)m.GetValue(null);
                else if (m.FieldType == typeof(decimal)) v = (double)(decimal)m.GetValue(null);
                else continue;

                AddGlobalConstant(m.Name, () => v);
            }

            foreach (var m in typeof(Math).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                if (m.ReturnType != typeof(double)) continue;
                var param = m.GetParameters();
                if (param.Length == 1)
                {
                    if (param[0].ParameterType == typeof(double))
                    {
                        AddGlobalFunction(m.Name, (Func<double, double>)Delegate.CreateDelegate(typeof(Func<double, double>), m));
                    }
                }
                else if (param.Length == 2)
                {
                    if (param[0].ParameterType == typeof(double) && param[1].ParameterType == typeof(double))
                    {
                        AddGlobalFunction(m.Name, (Func<double, double, double>)Delegate.CreateDelegate(typeof(Func<double, double, double>), m));
                    }
                    if (param[0].ParameterType == typeof(double) && param[1].ParameterType == typeof(int))
                    {
                        AddGlobalFunction(m.Name, (a, b) => (double)m.Invoke(null, new object[] { a, (int)b }));
                    }
                }
            }
        }

        public static void AddGlobalConstant(string name, Func<double> func)
        {
            GlobalConstants.Add(name.ToLower(), func);
        }

        public static void AddGlobalFunction(string name, Func<double, double> func)
        {
            GlobalFunctions1.Add(name.ToLower(), func);
        }

        public static void AddGlobalFunction(string name, Func<double, double, double> func)
        {
            GlobalFunctions2.Add(name.ToLower(), func);
        }

        public static void RemoveGlobal(string name)
        {
            name = name.ToLower();
            GlobalConstants.Remove(name);
            GlobalFunctions1.Remove(name);
            GlobalFunctions2.Remove(name);
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    MEMBER
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        // 상수
        protected readonly Dictionary<string, Func<double>> Constants = new Dictionary<string, Func<double>>();
        // 1항 함수
        protected readonly Dictionary<string, Func<double, double>> Functions1 = new Dictionary<string, Func<double, double>>();
        // 2항 함수
        protected readonly Dictionary<string, Func<double, double, double>> Functions2 = new Dictionary<string, Func<double, double, double>>();

        public BaseCalculator()
        {
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    ACTION
        //
        ///////////////////////////////////////////////////////////////////////////////////////


        public void AddConstant(string name, Func<double> func)
        {
            Constants.Add(name.ToLower(), func);
        }

        public void AddFunction(string name, Func<double, double> func)
        {
            Functions1.Add(name.ToLower(), func);
        }

        public void AddFunction(string name, Func<double, double, double> func)
        {
            Functions2.Add(name.ToLower(), func);
        }

        public void Remove(string name)
        {
            name = name.ToLower();
            Constants.Remove(name);
            Functions1.Remove(name);
            Functions2.Remove(name);
        }
    }

    public class StringCalculator : BaseCalculator
    {

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    DEFINE
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    STATIC
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    MEMBER
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public StringCalculator()
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    ACTION
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public double Calculate(string line)
        {
            line = ReplaceConstant(line);
            double result = Calc(line);
            return result;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    CALCULATOR
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        private string ReplaceConstant(string line)
        {
            var match = FindConstant.Match(line);
            if (match.Success)
            {
                string value = match.Value.ToLower();
                if (GlobalConstants.TryGetValue(value, out var v))
                {
                    line = line.Substring(0, match.Index) + v() + line.Substring(match.Index + match.Length);
                    return ReplaceConstant(line);
                }
                else if (Constants.TryGetValue(value, out v))
                {
                    line = line.Substring(0, match.Index) + v() + line.Substring(match.Index + match.Length);
                    return ReplaceConstant(line);
                }
                else
                {
                    throw new FormatException($"Unknown constant name '{match.Value}'.");
                }
            }
            return line;
        }

        private double Calc(string line)
        {
            Match match;
            for (int i = 0; i < DetectCalculates.Length; i++)
            {
                match = DetectCalculates[i].Match(line);
                if (match.Success)
                {
                    double d = MatchCalc(match.Value);
                    return InjectString(line, match, d);
                }
            }
            match = FindFunction1.Match(line);
            if (match.Success)
            {
                string funcName = match.Groups[1].Value;
                double a = double.Parse(match.Groups[2].Value);
                if (!string.IsNullOrWhiteSpace(funcName))
                {
                    funcName = funcName.ToLower();
                    if (Functions1.TryGetValue(funcName, out var func))
                    {
                        a = func(a);
                    }
                    else if (GlobalFunctions1.TryGetValue(funcName, out func))
                    {
                        a = func(a);
                    }
                    else
                    {
                        throw new FormatException($"Unknown function1 name '{match.Value}'.");
                    }
                }
                return InjectString(line, match, a);

            }
            match = FindFunc2.Match(line);
            if (match.Success)
            {
                string funcName = match.Groups[1].Value;
                double a = double.Parse(match.Groups[2].Value);
                double b = double.Parse(match.Groups[3].Value);
                if (!string.IsNullOrWhiteSpace(funcName))
                {
                    funcName = funcName.ToLower();
                    if (Functions2.TryGetValue(funcName, out var func))
                    {
                        a = func(a, b);
                    }
                    else if (GlobalFunctions2.TryGetValue(funcName, out func))
                    {
                        a = func(a, b);
                    }
                    else
                    {
                        throw new FormatException($"Unknown function2 name '{match.Value}'.");
                    }
                }
                return InjectString(line, match, a);

            }
            return double.Parse(line);
        }

        private double InjectString(string line, Match match, double d)
        {
            return Calc(line.Substring(0, match.Index) + d + line.Substring(match.Index + match.Length));
        }

        private double MatchCalc(string line)
        {
            var v = DetectCalculate.Match(line);
            if (v.Groups.Count == 4)
            {
                double a = MatchCalc(v.Groups[1].Value);
                double b = MatchCalc(v.Groups[3].Value);
                switch (v.Groups[2].Value)
                {
                    case "+": return a + b;
                    case "-": return a - b;
                    case "*": return a * b;
                    case "/": return a / b;
                    case "^": return Math.Pow(a, b);
                    case "%": return a % b;
                    default:
                        throw new FormatException($"Unkown Operator {v.Groups[2].Value} ({line}");
                }
            }
            else
            {
                v = OnlyNumber.Match(line);
                return double.Parse(v.Value);
            }
        }
    }
    public class FormatCalculator : BaseCalculator
    {
        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    DEFINE
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public abstract class ICalc
        {
            public virtual int Id { get; set; }
            public virtual double Value { get; }

            public override string ToString()
            {
                return "00" + Id;
            }
        }

        public class NumCalc : ICalc
        {
            protected double value;

            public NumCalc(double value)
            {
                this.value = value;
            }
            public override double Value => value;
        }

        public class FuncCalc : ICalc
        {
            protected Func<double> value;

            public FuncCalc(Func<double> value)
            {
                this.value = value;
            }

            public override double Value => value();
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    STATIC
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    MEMBER
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        private ICalc calculator;
        private List<ICalc> calcList = new List<ICalc>();

        public FormatCalculator()
        {
        }

        private ICalc AddCalc(ICalc calc)
        {
            calc.Id = calcList.Count;
            calcList.Add(calc);
            return calc;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    ACTION
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public void Setup(string line)
        {
            calculator = null;
            calcList.Clear();
            line = ReplaceConstant(line);
            calculator = MakeCalculator(line);
        }

        public double Calculate()
        {
            return calculator.Value;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    CALCULATOR
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        private string ReplaceConstant(string line)
        {
            var match = FindConstant.Match(line);
            if (match.Success)
            {
                string value = match.Value.ToLower();
                Func<double> v;
                if (!GlobalConstants.TryGetValue(value, out v) && !Constants.TryGetValue(value, out v))
                {
                    throw new FormatException($"Unknown constant name '{match.Value}'.");
                }

                var calc = AddCalc(new FuncCalc(v));
                line = line.Substring(0, match.Index) + calc + line.Substring(match.Index + match.Length);
                return ReplaceConstant(line);
            }
            return line;
        }

        private ICalc MakeCalculator(string line)
        {
            Match match;
            match = FindFunction1.Match(line);
            if (match.Success)
            {
                string funcName = match.Groups[1].Value;
                ICalc a = MatchCalc(match.Groups[2].Value);
                if (!string.IsNullOrWhiteSpace(funcName))
                {
                    ICalc temp = a;
                    funcName = funcName.ToLower();
                    if (Functions1.TryGetValue(funcName, out var func))
                    {
                        a = AddCalc(new FuncCalc(() => func(temp.Value)));
                    }
                    else if (GlobalFunctions1.TryGetValue(funcName, out func))
                    {
                        a = AddCalc(new FuncCalc(() => func(temp.Value)));
                    }
                    else
                    {
                        throw new FormatException($"Unknown function1 name '{match.Value}'.");
                    }
                }
                return InjectString(line, match, a);

            }
            match = FindFunc2.Match(line);
            if (match.Success)
            {
                string funcName = match.Groups[1].Value;
                ICalc a = MatchCalc(match.Groups[2].Value);
                ICalc b = MatchCalc(match.Groups[3].Value);
                if (!string.IsNullOrWhiteSpace(funcName))
                {
                    ICalc tempA = a;
                    ICalc tempB = b;
                    funcName = funcName.ToLower();
                    if (Functions2.TryGetValue(funcName, out var func))
                    {
                        a = AddCalc(new FuncCalc(() => func(tempA.Value, tempB.Value)));
                    }
                    else if (GlobalFunctions2.TryGetValue(funcName, out func))
                    {
                        a = AddCalc(new FuncCalc(() => func(tempA.Value, tempB.Value)));
                    }
                    else
                    {
                        throw new FormatException($"Unknown function2 name '{match.Value}'.");
                    }
                }
                return InjectString(line, match, a);
            }
            for (int i = 0; i < DetectCalculates.Length; i++)
            {
                match = DetectCalculates[i].Match(line);
                if (match.Success)
                {
                    ICalc d = MatchCalc(match.Value);
                    return InjectString(line, match, d);
                }
            }
            return MatchCalc(line);
        }

        private ICalc InjectString(string line, Match match, ICalc d)
        {
            return MakeCalculator(line.Substring(0, match.Index) + d + line.Substring(match.Index + match.Length));
        }

        private ICalc MatchCalc(string line)
        {
            var v = DetectCalculate.Match(line);
            if (v.Groups.Count == 4)
            {
                ICalc a = MatchCalc(v.Groups[1].Value);
                ICalc b = MatchCalc(v.Groups[3].Value);
                switch (v.Groups[2].Value)
                {
                    case "+": return AddCalc(new FuncCalc(() => a.Value + b.Value));
                    case "-": return AddCalc(new FuncCalc(() => a.Value - b.Value));
                    case "*": return AddCalc(new FuncCalc(() => a.Value * b.Value));
                    case "/": return AddCalc(new FuncCalc(() => a.Value / b.Value));
                    case "^": return AddCalc(new FuncCalc(() => Math.Pow(a.Value, b.Value)));
                    case "%": return AddCalc(new FuncCalc(() => a.Value % b.Value));
                    case ">>": return AddCalc(new FuncCalc(() => (double)((long)a.Value >> (int)b.Value)));
                    case "<<": return AddCalc(new FuncCalc(() => (double)((long)a.Value << (int)b.Value)));
                    case "&": return AddCalc(new FuncCalc(() => (double)((long)a.Value & (int)b.Value)));
                    case "|": return AddCalc(new FuncCalc(() => (double)((long)a.Value | (int)b.Value)));
                    default:
                        throw new FormatException($"Unkown Operator {v.Groups[2].Value} ({line}");
                }
            }
            else
            {
                if (line.StartsWith("00"))
                {
                    return calcList[int.Parse(line.Substring(2))];
                }
                else
                {
                    v = OnlyNumber.Match(line);
                    return AddCalc(new NumCalc(double.Parse(v.Value)));
                }
            }
        }
    }
}
