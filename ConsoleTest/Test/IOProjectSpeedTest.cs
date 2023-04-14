using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class IOProjectSpeedTest : TimeChecker
    {
        long tickOpenFile;
        long tickOpenFileCount;

        StringBuilder sb = new StringBuilder();
        public IOProjectSpeedTest()
        {
            //string drive = @"G:\내 드라이브\Programming\Unity\ForestDefenceVR";
            //drive = @"D:\ForestDefenceVR";
            string[] drives = File.ReadAllLines("target.txt");
            foreach(var drive in drives)
            {
                Search(drive);
            }

            File.WriteAllText("benchmark.tsv", sb.ToString(), Encoding.UTF8);
        }

        private void Search(string path)
        {
            var dt = data;
            Begin("GetFiles");
            var files = Directory.GetFiles(path);
            End(path, files.Length);

            foreach (var file in files)
            {
                Begin("OpenFile");
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.ReadByte();
                    long len = fs.Length;
                    End(file, len);

                    Begin("ReadFile");
                    long readed = 0;
                    int read;
                    while (readed < len)
                    {
                        read = fs.Read(dt, 0, Math.Min(dt.Length, (int)(len - readed)));
                        if (read == 0) break;
                        readed += read;
                    }
                    End(file, len);
                }
            }

            Begin("GetDirs");
            var dirs = Directory.GetDirectories(path);
            End(path, dirs.Length);

            foreach (var dir in dirs)
            {
                Search(dir);
            }
        }

        private byte[] data = new byte[1024 * 1024];
        private string name;
        private Stopwatch stopwatch = new Stopwatch();
        private void Begin(string name)
        {
            this.name = name;
            stopwatch.Restart();
        }

        private void End(params object[] datas)
        {
            stopwatch.Stop();
            sb.AppendLine(stopwatch.ElapsedTicks + "\t" + name + "\t" + string.Join('\t', datas));
        }
    }
}
