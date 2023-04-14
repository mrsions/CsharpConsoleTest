using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest.Test
{
    public class StringFormatDictionary
    {
        public StringFormatDictionary(string[] args)
        {
            string format = "v:{Name} / {{ HP : {Hp}}}";
            string message = ToFormat(format, new Dictionary<string, string> { ["Name"] = "Sions", ["Hp"] = "52" });
            Console.Write(message);
        }


        public static string ToFormat<T>(string format, IDictionary<string, T> dictionary)
        {
            StringBuilder sb = new StringBuilder(format.Length * 2);
            StringBuilder sb2 = new StringBuilder(format.Length);

            char openChar = (char)0;

            for (int i = 0, len = format.Length; i < len; i++)
            {
                char c = format[i];
                if (c == '{')
                {
                    if (openChar == 0)
                    {
                        openChar = c;
                    }
                    else if (openChar == '{')
                    {
                        sb.Append('{');
                        openChar = (char)0;
                    }
                    else
                    {
                        throw new FormatException($"Input string was not in a correct format. ({i}/{format.Length})");
                    }
                }
                else if (c == '}')
                {
                    if (openChar == 0)
                    {
                        openChar = c;
                    }
                    else if (openChar == '}')
                    {
                        sb.Append('}');
                        openChar = (char)0;
                    }
                    else if (openChar == '{')
                    {
                        if (sb2.Length > 0)
                        {
                            string ssb2 = sb2.ToString();
                            if (dictionary.TryGetValue(ssb2, out var rst))
                            {
                                sb.Append(rst);
                            }
                            else
                            {
                                sb.Append('{').Append(ssb2).Append('}');
                            }
                            sb2.Clear();
                            openChar = (char)0;
                        }
                        else
                        {
                            throw new FormatException($"Input string was not in a correct format. ({i - 1}/{format.Length})");
                        }
                    }
                    else
                    {
                        throw new FormatException($"Input string was not in a correct format. ({i}/{format.Length})");
                    }
                }
                else
                {
                    if (openChar != 0)
                    {
                        sb2.Append(c);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }

            return sb.ToString();
        }

    }
}
