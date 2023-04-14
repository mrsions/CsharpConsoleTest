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
    public class IOSpeedTest : TimeChecker
    {
        public IOSpeedTest()
        {
            string drive = @"G:/내 드라이브/";

            using (var fs = new FileStream(drive + "log.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (var bs = new StreamWriter(fs))
            {
                while (true)
                {
                    bs.WriteLine(DateTime.Now.ToString());
                    bs.Flush();
                    Thread.Sleep(1000);
                }
            }

            drive = @"D:/";
            drive = @"C:\Users\mrsio/";

            int cnt = 10;

            byte[][] files = new byte[3][];
            files[0] = new byte[4 * 1024]; // 4KB
            files[1] = new byte[100 * 1024]; // 100KB
            files[2] = new byte[1024 * 1024]; // 1MB
            //files[3] = new byte[10 * 1024 * 1024]; // 10MB
            //files[4] = new byte[100 * 1024 * 1024]; // 100MB
            foreach (var file in files)
            {
                new Random().NextBytes(file);
            }

            HashSet<string> dirs = new HashSet<string>();
            HashSet<string> fileNames = new HashSet<string>();
            for (int i = 0; i < cnt; i++)
            {
                string fileName;
                do
                {
                    fileName = drive + "SpeedTest/" + Guid.NewGuid() + "/" + Guid.NewGuid() + ".bin";
                }
                while (!fileNames.Add(fileName));

                string dir = Path.GetDirectoryName(fileName);
                try { Directory.Delete(dir, true); } catch { }
                dirs.Add(dir);
            }

            while (true)
            {
                BeginSample("C-Dir");
                foreach (var dir in dirs)
                {
                    Directory.CreateDirectory(dir);
                }
                EndSample();

                BeginSample("C-4K");
                foreach (var fileName in fileNames)
                {
                    File.WriteAllBytes(fileName, files[0]);
                }
                EndSample();

                BeginSample("R-4K");
                foreach (var fileName in fileNames)
                {
                    File.ReadAllBytes(fileName);
                }
                EndSample();

                BeginSample("D-4K");
                foreach (var fileName in fileNames)
                {
                    File.Delete(fileName);
                }
                EndSample();

                BeginSample("C-100K");
                foreach (var fileName in fileNames)
                {
                    File.WriteAllBytes(fileName, files[1]);
                }
                EndSample();

                BeginSample("R-100K");
                foreach (var fileName in fileNames)
                {
                    File.ReadAllBytes(fileName);
                }
                EndSample();

                BeginSample("D-100K");
                foreach (var fileName in fileNames)
                {
                    File.Delete(fileName);
                }
                EndSample();

                BeginSample("C-1M");
                foreach (var fileName in fileNames)
                {
                    File.WriteAllBytes(fileName, files[2]);
                }
                EndSample();

                BeginSample("R-1M");
                foreach (var fileName in fileNames)
                {
                    File.ReadAllBytes(fileName);
                }
                EndSample();

                BeginSample("D-1M");
                foreach (var fileName in fileNames)
                {
                    File.Delete(fileName);
                }
                EndSample();

                BeginSample("D-Dir");
                foreach (var dir in dirs)
                {
                    Directory.Delete(dir);
                }
                EndSample();


                PrintSamples();
            }
        }
    }
}
