using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class DotnetEfQueryToSql
    {
        public DotnetEfQueryToSql()
        {
            Console.WriteLine("데이터를 입력 해 주세요");
            Console.WriteLine("(엔터를 두번 치셔야 다음으로 넘어갑니다.)");

            string args = ReadBlock();

            Console.WriteLine("쿼리를 입력 해 주세요.");
            Console.WriteLine("(엔터를 두번 치셔야 다음으로 넘어갑니다.)");

            string sql = ReadBlock();

            foreach (Match match in Regex.Matches(args, @"(@[a-zA-Z0-9_]+)='([^']+)'( \(Size = \d+\))?"))
            {
                if (match.Groups[3].Success)
                {
                    sql = Regex.Replace(sql, @$"{match.Groups[1].Value}\b", $"'{match.Groups[2].Value}'");
                }
                else
                {
                    sql = Regex.Replace(sql, @$"{match.Groups[1].Value}\b", match.Groups[2].Value);
                }
            }

            Console.WriteLine(sql);
        }

        private static string ReadBlock()
        {
            string args = "";
            while (!args.EndsWith("\n\n"))
            {
                args += Console.ReadLine().Trim() + "\n";
            }
            return args;
        }
    }
}
