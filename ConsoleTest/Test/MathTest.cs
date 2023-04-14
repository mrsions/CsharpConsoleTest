using System;

namespace ConsoleTest
{
    public class MathTest : TimeChecker
    {
        public MathTest()
        {
            const int cnt = 10000000;
            float v = (float)Math.PI;
            float c = 0;
            while (true)
            {
                //BeginSample("A");
                //for (int i = 0; i < cnt; i++)
                //{
                //    c = (float)Math.Round(v, 2);
                //}

                //EndSample();
                //BeginSample("B");
                //for (int i = 0; i < cnt; i++)
                //{
                //    //c = SMath.Round(v);
                //}
                //EndSample();
                //BeginSample("C");
                //for (int i = 0; i < cnt; i++)
                //{
                //    c = SMath.Round2(v, 2);
                //}
                //EndSample();
                //BeginSample("D");
                //for (int i = 0; i < cnt; i++)
                //{
                //    c = SMath.Round3(v, 2);
                //}
                //EndSample();
                //Console.WriteLine("--------------------------" + c);
                //PrintSamples();
            }
        }
    }


    public static class SMath
    {
        public static float PI_F = (float)Math.PI; // 
        public static double PI_D = Math.PI;
        public static float E_F = (float)Math.E; // 
        public static double E_D = Math.E;
        public static System.Random rnd = new System.Random();

        //public static Vector3 SubtractAngles(Vector3 a, Vector3 b)
        //{
        //    return (Quaternion.Euler(a).Inverse() * Quaternion.Euler(b)).eulerAngles;
        //}

        //public static Vector3 StabilizeAngle(Vector3 eulerAngles)
        //{
        //    eulerAngles.x = StabilizeAngle(eulerAngles.x);
        //    eulerAngles.y = StabilizeAngle(eulerAngles.y);
        //    eulerAngles.z = StabilizeAngle(eulerAngles.z);
        //    return eulerAngles;
        //}

        public static float StabilizeAngle(float x)
        {
            return x > 180 ? x - 360 : x;
        }
        public static byte Max(byte a, byte b) => (a > b ? a : b);
        public static ushort Max(ushort a, ushort b) => (a > b ? a : b);
        public static uint Max(uint a, uint b) => (a > b ? a : b);
        public static ulong Max(ulong a, ulong b) => (a > b ? a : b);
        public static sbyte Max(sbyte a, sbyte b) => (a > b ? a : b);
        public static short Max(short a, short b) => (a > b ? a : b);
        public static int Max(int a, int b) => (a > b ? a : b);
        public static long Max(long a, long b) => (a > b ? a : b);
        public static float Max(float a, float b) => (a > b ? a : b);
        public static double Max(double a, double b) => (a > b ? a : b);
        public static decimal Max(decimal a, decimal b) => (a > b ? a : b);
        public static byte Min(byte a, byte b) => (a < b ? a : b);
        public static ushort Min(ushort a, ushort b) => (a < b ? a : b);
        public static uint Min(uint a, uint b) => (a < b ? a : b);
        public static ulong Min(ulong a, ulong b) => (a < b ? a : b);
        public static sbyte Min(sbyte a, sbyte b) => (a < b ? a : b);
        public static short Min(short a, short b) => (a < b ? a : b);
        public static int Min(int a, int b) => (a < b ? a : b);
        public static long Min(long a, long b) => (a < b ? a : b);
        public static float Min(float a, float b) => (a < b ? a : b);
        public static double Min(double a, double b) => (a < b ? a : b);
        public static decimal Min(decimal a, decimal b) => (a < b ? a : b);
        public static byte Clamp(byte a, byte min, byte max) => (a < min ? min : (a > max ? max : a));
        public static ushort Clamp(ushort a, ushort min, ushort max) => (a < min ? min : (a > max ? max : a));
        public static uint Clamp(uint a, uint min, uint max) => (a < min ? min : (a > max ? max : a));
        public static ulong Clamp(ulong a, ulong min, ulong max) => (a < min ? min : (a > max ? max : a));
        public static sbyte Clamp(sbyte a, sbyte min, sbyte max) => (a < min ? min : (a > max ? max : a));
        public static short Clamp(short a, short min, short max) => (a < min ? min : (a > max ? max : a));
        public static int Clamp(int a, int min, int max) => (a < min ? min : (a > max ? max : a));
        public static long Clamp(long a, long min, long max) => (a < min ? min : (a > max ? max : a));
        public static float Clamp(float a, float min, float max) => (a < min ? min : (a > max ? max : a));
        public static double Clamp(double a, double min, double max) => (a < min ? min : (a > max ? max : a));
        public static decimal Clamp(decimal a, decimal min, decimal max) => (a < min ? min : (a > max ? max : a));
        public static byte Clamp01(byte a) => (a < 0 ? (byte)0 : (a > 1 ? (byte)1 : a));
        public static ushort Clamp01(ushort a) => (a < 0 ? (ushort)0 : (a > 1 ? (ushort)1 : a));
        public static uint Clamp01(uint a) => (a < 0 ? (uint)0 : (a > 1 ? (uint)1 : a));
        public static ulong Clamp01(ulong a) => (a < 0 ? (ulong)0 : (a > 1 ? (ulong)1 : a));
        public static sbyte Clamp01(sbyte a) => (a < 0 ? (sbyte)0 : (a > 1 ? (sbyte)1 : a));
        public static short Clamp01(short a) => (a < 0 ? (short)0 : (a > 1 ? (short)1 : a));
        public static int Clamp01(int a) => (a < 0 ? (int)0 : (a > 1 ? (int)1 : a));
        public static long Clamp01(long a) => (a < 0 ? (long)0 : (a > 1 ? (long)1 : a));
        public static float Clamp01(float a) => (a < 0 ? (float)0 : (a > 1 ? (float)1 : a));
        public static double Clamp01(double a) => (a < 0 ? (double)0 : (a > 1 ? (double)1 : a));
        public static decimal Clamp01(decimal a) => (a < 0 ? (decimal)0 : (a > 1 ? (decimal)1 : a));
        public static byte Abs(byte a) => a;
        public static ushort Abs(ushort a) => a;
        public static uint Abs(uint a) => a;
        public static ulong Abs(ulong a) => a;
        public static byte Sign(byte a) => (a == 0 ? (byte)0 : a);
        public static ushort Sign(ushort a) => (a == 0 ? (ushort)0 : a);
        public static uint Sign(uint a) => (a == 0 ? (uint)0 : a);
        public static ulong Sign(ulong a) => (a == 0 ? (ulong)0 : a);
        public static sbyte Abs(sbyte a) => (a < 0 ? (sbyte)-a : a);
        public static short Abs(short a) => (a < 0 ? (short)-a : a);
        public static int Abs(int a) => (a < 0 ? (int)-a : a);
        public static long Abs(long a) => (a < 0 ? (long)-a : a);
        public static float Abs(float a) => (a < 0 ? (float)-a : a);
        public static double Abs(double a) => (a < 0 ? (double)-a : a);
        public static decimal Abs(decimal a) => (a < 0 ? (decimal)-a : a);
        public static sbyte Sign(sbyte a) => (a == 0 ? (sbyte)0 : (a > 0 ? (sbyte)1 : (sbyte)-1));
        public static short Sign(short a) => (a == 0 ? (short)0 : (a > 0 ? (short)1 : (short)-1));
        public static int Sign(int a) => (a == 0 ? (int)0 : (a > 0 ? (int)1 : (int)-1));
        public static long Sign(long a) => (a == 0 ? (long)0 : (a > 0 ? (long)1 : (long)-1));
        public static float Sign(float a) => (a == 0 ? (float)0 : (a > 0 ? (float)1 : (float)-1));
        public static double Sign(double a) => (a == 0 ? (double)0 : (a > 0 ? (double)1 : (double)-1));
        public static decimal Sign(decimal a) => (a == 0 ? (decimal)0 : (a > 0 ? (decimal)1 : (decimal)-1));
        public static float Floor(float a) => (int)a;
        public static double Floor(double a) => (int)a;
        public static decimal Floor(decimal a) => decimal.Floor(a);
        public static float Ceil(float a) => (float)Math.Ceiling(a);
        public static double Ceil(double a) => (double)Math.Ceiling(a);
        public static decimal Ceil(decimal a) => decimal.Ceiling(a);
        public static float Round(float a) => (float)Math.Round(a);
        public static double Round(double a) => (double)Math.Round(a);
        public static decimal Round(decimal a) => decimal.Round(a);
        public static float Round(float a, int digits) => (float)Math.Round(a, digits);
        public static double Round(double a, int digits) => (double)Math.Round(a, digits);
        public static decimal Round(decimal a, int digits) => decimal.Round(a, digits);
        public static int FloorToInt(float a) => (int)a;
        public static int FloorToInt(double a) => (int)a;
        public static int FloorToInt(decimal a) => (int)decimal.Floor(a);
        public static int CeilToInt(float a) => (int)(float)Math.Ceiling(a);
        public static int CeilToInt(double a) => (int)(double)Math.Ceiling(a);
        public static int CeilToInt(decimal a) => (int)decimal.Ceiling(a);
        public static int RoundToInt(float a) => (int)(float)Math.Round(a);
        public static int RoundToInt(double a) => (int)(double)Math.Round(a);
        public static int RoundToInt(decimal a) => (int)decimal.Round(a);
        public static int RoundToInt(float a, int digits) => (int)(float)Math.Round(a, digits);
        public static int RoundToInt(double a, int digits) => (int)(double)Math.Round(a, digits);
        public static int RoundToInt(decimal a, int digits) => (int)decimal.Round(a, digits);

        public static bool RandomBool() => rnd.Next(0, 2) == 0;
        public static float Random() => (float)rnd.NextDouble();
        public static float Random(float min, float max) => min + rnd.Next() * (max - min);
        public static float Random(float max) => rnd.Next() * max;
        public static double RandomDouble() => rnd.NextDouble();

        public static int RandomInt(int min, int max) => rnd.Next(min, max);
        public static int RandomInt(int max) => rnd.Next(0, max);
        public static int RandomInt() => rnd.Next(int.MinValue, int.MaxValue);

        public static void Random(byte[] data) => rnd.NextBytes(data);
    }
}
