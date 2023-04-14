using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleTest
{
    public class LogTest
    {
        public LogTest()
        {
            int pid = new Random().Next();
            int tid = new Random().Next();
            string type = "Test";
            string tag = "Temp";

            string path1 = "logs/current.log";
            string path2 = $"logs/{DateTime.Now:yyyyMMdd_HHmmssfff}.log";
            //using (var currentLogStream = new StreamWriter(new FileStream(, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)))
            //using (var logStream = new StreamWriter(new FileStream(, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {
                while (true)
                {
                    string msg = $"{DateTime.Now:MM-dd HH:mm:ss.fff} {pid,5} {tid,5} {type} {tag,-40} : {Guid.NewGuid().ToString()}";
                    using (var logStream = new StreamWriter(new FileStream(path1, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                    {
                        logStream.WriteLine(msg);
                        logStream.Flush();
                    }
                    using (var logStream = new StreamWriter(new FileStream(path2, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                    {
                        logStream.WriteLine(msg);
                        logStream.Flush();
                    }

                    Thread.Sleep(100);
                }
            }
        }

    }
}