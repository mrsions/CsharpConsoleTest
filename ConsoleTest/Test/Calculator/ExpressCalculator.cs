using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;

namespace ConsoleTest
{
    public class StringCalculatorTest2
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

    public class ExpressCalculator
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

        // 상수
        protected static Dictionary<string, Func<double>> GlobalConstants = new Dictionary<string, Func<double>>();

        // 2항 함수
        protected static Dictionary<string, Func<double, double, double>> GlobalFunctions2 = new Dictionary<string, Func<double, double, double>>();

        // 1항 함수
        protected static Dictionary<string, Func<double, double>> GlobalFunctions1 = new Dictionary<string, Func<double, double>>();

        static ExpressCalculator()
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

        private ICalc calculator;
        private List<ICalc> calcList = new List<ICalc>();

        public ExpressCalculator()
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
        }

        public double Calculate()
        {
            return 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    CALCULATOR
        //
        ///////////////////////////////////////////////////////////////////////////////////////

    }
}
