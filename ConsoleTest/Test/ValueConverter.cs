using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleTest
{
    public class TypeConverterTest : TimeChecker
    {
        public TypeConverterTest()
        {
            //var addTypes = GetType().Assembly.GetTypes().Where(v => v.FullName.StartsWith(typeof(TypeConverter).FullName + "+")).ToArray();
            //StringBuilder sbEx = new StringBuilder();
            //StringBuilder sbWr = new StringBuilder();
            //foreach (var type in addTypes)
            //{
            //    //public static void Write(StringBuilder sb, byte val) => sb.Append(val);

            //    #region Exporter
            //    sbEx.Append("public static void Write(StringBuilder sb, " + type.Name + " val) => sb.Append($\"");
            //    var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            //    bool first = true;
            //    foreach (var field in fields)
            //    {
            //        if (first) first = false;
            //        else sbEx.Append(", ");
            //        sbEx.Append($"{{val.{field.Name}}}");
            //    }
            //    sbEx.Append("\")\r\n");
            //    #endregion

            //    sbWr.AppendLine($"if (type == typeof({type.Name})) Write(sb, ({type.Name})val);");
            //}

            List<object> objs = new List<object>
            {
                (sbyte)34,
                (byte)12,
                (char)1234,
                (short)1234,
                (ushort)1234,
                (int)1234,
                (uint)1234,
                (long)1234,
                (ulong)1234,
                (bool)true,
                (float)1234,
                (double)1234,
                (decimal)1234,
                "sadfklasjflk",
                new Bounds      (),
                new BoundsInt   (),
                new Plane       (),
                new Ray         (),
                new Ray2D       (),
                new Rect        (),
                new RectInt     (),
                new Color       (),
                new Color32     (),
                new Matrix4x4   (),
                new Vector3     (),
                new Quaternion  (),
                new Vector2     (),
                new Vector2Int  (),
                new Vector3Int  (),
                new Vector4     (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new LayerMask   (),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
                new object(),
            };
            StringBuilder sb = new StringBuilder(10241024);

            int cnt = 100000;
            while (true)
            {
                Proc("Write", cnt, () =>
                {
                    sb.Clear();
                    for (int j = 0; j < objs.Count; j++)
                    {
                        var o = objs[j];
                        ValueConverter.Write(sb, o.GetType(), o);
                        sb.AppendLine();
                    }
                }); ;

                Proc("Write2", cnt, () =>
                {
                    sb.Clear();
                    for (int j = 0; j < objs.Count; j++)
                    {
                        var o = objs[j];
                        ValueConverter.Write2(sb, o.GetType(), o);
                        sb.AppendLine();
                    }
                }); ;

                PrintSamples();
                //ClearAll();
            }
        }
    }

    public static class ValueConverter
    {
        public static Dictionary<Type, Action<StringBuilder, object>> ValueExporters = new Dictionary<Type, Action<StringBuilder, object>>();
        public static Dictionary<Type, Func<Type, string, object>> ValueImporters = new Dictionary<Type, Func<Type, string, object>>();

        static ValueConverter()
        {
            ValueExporters[typeof(sbyte)] = (sb, v) => Write(sb, (sbyte)v);
            ValueExporters[typeof(byte)] = (sb, v) => Write(sb, (byte)v);
            ValueExporters[typeof(char)] = (sb, v) => Write(sb, (char)v);
            ValueExporters[typeof(short)] = (sb, v) => Write(sb, (short)v);
            ValueExporters[typeof(ushort)] = (sb, v) => Write(sb, (ushort)v);
            ValueExporters[typeof(int)] = (sb, v) => Write(sb, (int)v);
            ValueExporters[typeof(uint)] = (sb, v) => Write(sb, (uint)v);
            ValueExporters[typeof(long)] = (sb, v) => Write(sb, (long)v);
            ValueExporters[typeof(ulong)] = (sb, v) => Write(sb, (ulong)v);
            ValueExporters[typeof(bool)] = (sb, v) => Write(sb, (bool)v);
            ValueExporters[typeof(float)] = (sb, v) => Write(sb, (float)v);
            ValueExporters[typeof(double)] = (sb, v) => Write(sb, (double)v);
            ValueExporters[typeof(decimal)] = (sb, v) => Write(sb, (decimal)v);
            ValueExporters[typeof(Bounds)] = (sb, val) => Write(sb, (Bounds)val);
            ValueExporters[typeof(BoundsInt)] = (sb, val) => Write(sb, (BoundsInt)val);
            ValueExporters[typeof(Plane)] = (sb, val) => Write(sb, (Plane)val);
            ValueExporters[typeof(Ray)] = (sb, val) => Write(sb, (Ray)val);
            ValueExporters[typeof(Ray2D)] = (sb, val) => Write(sb, (Ray2D)val);
            ValueExporters[typeof(Rect)] = (sb, val) => Write(sb, (Rect)val);
            ValueExporters[typeof(RectInt)] = (sb, val) => Write(sb, (RectInt)val);
            ValueExporters[typeof(Color)] = (sb, val) => Write(sb, (Color)val);
            ValueExporters[typeof(Color32)] = (sb, val) => Write(sb, (Color32)val);
            ValueExporters[typeof(Matrix4x4)] = (sb, val) => Write(sb, (Matrix4x4)val);
            ValueExporters[typeof(Vector3)] = (sb, val) => Write(sb, (Vector3)val);
            ValueExporters[typeof(Quaternion)] = (sb, val) => Write(sb, (Quaternion)val);
            ValueExporters[typeof(Vector2)] = (sb, val) => Write(sb, (Vector2)val);
            ValueExporters[typeof(Vector2Int)] = (sb, val) => Write(sb, (Vector2Int)val);
            ValueExporters[typeof(Vector3Int)] = (sb, val) => Write(sb, (Vector3Int)val);
            ValueExporters[typeof(Vector4)] = (sb, val) => Write(sb, (Vector4)val);
            ValueExporters[typeof(LayerMask)] = (sb, val) => Write(sb, (LayerMask)val);
        }

        public static void Write(StringBuilder sb, object val)
        {
            if (val != null)
            {
                Write(sb, val.GetType(), val);
            }
        }
        public static void Write(StringBuilder sb, Type type, object val)
        {
            if (type == typeof(string)) Write(sb, (string)val);
            else if (type == typeof(byte)) Write(sb, (byte)val);
            else if (type == typeof(char)) Write(sb, (char)val);
            else if (type == typeof(int)) Write(sb, (int)val);
            else if (type == typeof(bool)) Write(sb, (bool)val);
            else if (type == typeof(float)) Write(sb, (float)val);
            //if (type == typeof(sbyte)) Write(sb, (sbyte)val);
            //else if (type == typeof(short)) Write(sb, (short)val);
            //else if (type == typeof(ushort)) Write(sb, (ushort)val);
            //else if (type == typeof(uint)) Write(sb, (uint)val);
            //else if (type == typeof(long)) Write(sb, (long)val);
            //else if (type == typeof(ulong)) Write(sb, (ulong)val);
            //else if (type == typeof(double)) Write(sb, (double)val);
            //else if (type == typeof(decimal)) Write(sb, (decimal)val);
            else if (type == typeof(Color)) Write(sb, (Color)val);
            else if (type == typeof(Vector3)) Write(sb, (Vector3)val);
            else if (type == typeof(Quaternion)) Write(sb, (Quaternion)val);
            //else if (type == typeof(Bounds)) Write(sb, (Bounds)val);
            //else if (type == typeof(BoundsInt)) Write(sb, (BoundsInt)val);
            //else if (type == typeof(Plane)) Write(sb, (Plane)val);
            //else if (type == typeof(Ray)) Write(sb, (Ray)val);
            //else if (type == typeof(Ray2D)) Write(sb, (Ray2D)val);
            //else if (type == typeof(Rect)) Write(sb, (Rect)val);
            //else if (type == typeof(RectInt)) Write(sb, (RectInt)val);
            //else if (type == typeof(Color32)) Write(sb, (Color32)val);
            //else if (type == typeof(Matrix4x4)) Write(sb, (Matrix4x4)val);
            //else if (type == typeof(Vector2)) Write(sb, (Vector2)val);
            //else if (type == typeof(Vector2Int)) Write(sb, (Vector2Int)val);
            //else if (type == typeof(Vector3Int)) Write(sb, (Vector3Int)val);
            //else if (type == typeof(Vector4)) Write(sb, (Vector4)val);
            //else if (type == typeof(LayerMask)) Write(sb, (LayerMask)val);
            else if (ValueExporters.TryGetValue(type, out var writerAction))
            {
                writerAction(sb, val);
            }
            else
            {
                sb.Append(val);
            }
        }
        public static void Write2(StringBuilder sb, Type type, object val)
        {
            if (ValueExporters.TryGetValue(type, out var writerAction))
            {
                writerAction(sb, val);
            }
            else
            {
                sb.Append(val);
            }
        }
        public static StringBuilder Write(StringBuilder sb, byte[] val) => sb.Append(Convert.ToBase64String(val));
        public static StringBuilder Write(StringBuilder sb, string val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, sbyte val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, byte val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, char val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, short val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, ushort val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, int val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, uint val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, long val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, ulong val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, bool val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, float val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, double val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, decimal val) => sb.Append(val);
        public static StringBuilder Write(StringBuilder sb, Color val) => Write(sb, (Color32)val);
        public static StringBuilder Write(StringBuilder sb, Vector3 val) => sb.Append($"{val.x} {val.y} {val.z}");
        public static StringBuilder Write(StringBuilder sb, Quaternion val) => sb.Append($"{val.x} {val.y} {val.z} {val.w}");
        public static StringBuilder Write(StringBuilder sb, Vector2 val) => sb.Append($"{val.x} {val.y}");
        public static StringBuilder Write(StringBuilder sb, Vector2Int val) => sb.Append($"{val.x} {val.y}");
        public static StringBuilder Write(StringBuilder sb, Vector3Int val) => sb.Append($"{val.x} {val.y} {val.z}");
        public static StringBuilder Write(StringBuilder sb, Vector4 val) => sb.Append($"{val.x} {val.y} {val.z} {val.w}");
        public static StringBuilder Write(StringBuilder sb, LayerMask val) => sb.Append($"{val.value}");
        public static StringBuilder Write(StringBuilder sb, Matrix4x4 val) => sb.Append($"{val.m00} {val.m10} {val.m20} {val.m30} {val.m01} {val.m11} {val.m21} {val.m31} {val.m02} {val.m12} {val.m22} {val.m32} {val.m03} {val.m13} {val.m23} {val.m33}");

        public static StringBuilder Write(StringBuilder sb, Bounds val)
        {
            Write(sb, val.center).Append(' ');
            return Write(sb, val.extents);
        }
        public static StringBuilder Write(StringBuilder sb, BoundsInt val)
        {
            Write(sb, val.position).Append(",");
            return Write(sb, val.size);
        }
        public static StringBuilder Write(StringBuilder sb, Plane val)
        {
            Write(sb, val.normal).Append(' ');
            return Write(sb, val.distance);
        }
        public static StringBuilder Write(StringBuilder sb, Ray val)
        {
            Write(sb, val.origin).Append(' ');
            return Write(sb, val.direction);
        }
        public static StringBuilder Write(StringBuilder sb, Ray2D val)
        {
            Write(sb, val.origin).Append(' ');
            return Write(sb, val.direction);
        }
        public static StringBuilder Write(StringBuilder sb, Rect val)
        {
            Write(sb, val.xMin).Append(' ');
            Write(sb, val.yMin).Append(' ');
            Write(sb, val.width).Append(' ');
            return Write(sb, val.height);
        }
        public static StringBuilder Write(StringBuilder sb, RectInt val)
        {
            Write(sb, val.xMin).Append(' ');
            Write(sb, val.yMin).Append(' ');
            Write(sb, val.width).Append(' ');
            return Write(sb, val.height);
        }
        public static StringBuilder Write(StringBuilder sb, Color32 val)
        {
            sb.Append('#');
            WriteHex(sb, val.r);
            WriteHex(sb, val.g);
            WriteHex(sb, val.b);
            if (val.a != 255)
            {
                WriteHex(sb, val.a);
            }
            return sb;
        }

        private static char[] DEC2HEX = "0123456789ABCDEF".ToCharArray();
        public static StringBuilder WriteHex(StringBuilder sb, byte val)
        {
            sb.Append(DEC2HEX[(val >> 4) & 0xF]);
            return sb.Append(DEC2HEX[val & 0xF]);
        }

    }

    public struct Bounds
    {
        public Vector3 center;
        public Vector3 extents;
    }
    public struct BoundsInt
    {
        public Vector3Int position;
        public Vector3Int size;
    }
    public struct Plane
    {
        public Vector3 normal;
        public Single distance;
    }
    public struct Ray
    {
        public Vector3 origin;
        public Vector3 direction;
    }
    public struct Ray2D
    {
        public Vector2 origin;
        public Vector2 direction;
    }
    public struct Rect
    {
        public Single width;
        public Single height;
        public Single xMin;
        public Single yMin;
    }
    public struct RectInt
    {
        public Int32 width;
        public Int32 height;
        public Int32 xMin;
        public Int32 yMin;
    }

    public struct Color
    {
        public Single r;
        public Single g;
        public Single b;
        public Single a;
    }
    public struct Color32
    {
        public Byte r;
        public Byte g;
        public Byte b;
        public Byte a;

        public static implicit operator Color32(Color col)
        {
            return new Color32
            {
                r = (byte)(col.r * 255),
                g = (byte)(col.g * 255),
                b = (byte)(col.b * 255),
                a = (byte)(col.a * 255),
            };
        }
    }
    public struct Matrix4x4
    {
        public Single m00;
        public Single m10;
        public Single m20;
        public Single m30;
        public Single m01;
        public Single m11;
        public Single m21;
        public Single m31;
        public Single m02;
        public Single m12;
        public Single m22;
        public Single m32;
        public Single m03;
        public Single m13;
        public Single m23;
        public Single m33;
    }
    public struct Vector3
    {
        public Single x, y, z;
    }
    public struct Quaternion
    {
        public Single x, y, z, w;
    }
    public struct Vector2
    {
        public Single x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public struct Vector2Int
    {
        public Int32 x;
        public Int32 y;
    }
    public struct Vector3Int
    {
        public Int32 x;
        public Int32 y;
        public Int32 z;
    }
    public struct Vector4
    {
        public Single x, y, z, w;
    }
    public struct LayerMask
    {
        public Int32 value;
    }

}
