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

namespace ConsoleTest
{
    public class KeystoreTest
    {
        static string[] keylist = @"D:\Work\Test\AIO-Sample-VideoPlayer\user.keystore
D:\Work\RW.Ras\Project\Unity\RAS.Connect\racon_priv.keystore
D:\Work\RW.Ras\Project\Unity\RAS.Connect\racon.keystore
D:\Work\RW.Ras\Project\Unity\123qwe.keystore
D:\Work\RW.Ras\Project\Android\NatCamCustom\Android_NatCamCustom\123qwe.keystore
D:\Work\RW.MindColoring\Project\MindColoring\user.keystore
D:\Work\RW.HiLang\Project\HiLang\hilangxr.keystore
D:\Work\Project\RWA\RWA\ClientApp\node_modules\react-native\template\android\app\debug.keystore
D:\Work\Project\RealwithColoring\user.keystore
D:\Work\Project\OEM\XMES-VideoPlayer\user.keystore
D:\Work\Project\OEM\iScreamHomeLearn\2019.08.RuralVillage\Resources\아이스크림홈런 작업파일(190812)\0_IscreamLgTest\user.keystore
D:\Work\Project\OEM\Insedu\SampleProject2\123qwe.keystore
D:\Work\Project\OEM\HistoryMuseum\123qwe.keystore
D:\Work\Project\OEM\Ani2Art\191212-ArBook\user.keystore
D:\Work\Project\OEM\0_NE_sample\NExr.keystore
D:\Work\Project\NosyAndBeanstalk\user.keystore
D:\Work\Project\HiLang\hilangxr.keystore
D:\Work\Project\9.ETC\RwColoring\rwcoloring.keystore
D:\Work\Project\9.ETC\RealwithApps\apps.keystore
D:\Work\Project\9.ETC\Coloring360\hienglishxr.keystore
D:\Work\Project\9.ETC\AndroidMindColoringXR\user.keystore
D:\Work\Project\123qwe.keystore
D:\Work\OEM\TheTeacherByeon\user.keystore
D:\Work\OEM.ISHL.1\2019.08.RuralVillage\Resources\아이스크림홈런 작업파일(190812)\0_IscreamLgTest\user.keystore
D:\Sions\Android\Sionsbeat\sionskey.keystore".Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        public KeystoreTest()
        {
            foreach (var path in keylist)
            {
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine(path);

                var dir = Path.GetDirectoryName(path);
                var proj = Path.Combine(dir, "project_default_key.txt");
                if (!File.Exists(proj)) continue;

                string[] passwords = File.ReadAllLines(proj);
                if (passwords.Length < 3) continue;

                var proc = Process.Start(new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\Java\jdk1.8.0_111\bin\keytool.exe",
                    Arguments = $"-exportcert -alias {passwords[1]} -storepass {passwords[0]} -list -v -keystore \"{path}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
                proc.WaitForExit();

                string output = proc.StandardOutput.ReadToEnd();
                string error = proc.StandardError.ReadToEnd();
                Console.WriteLine("----out");
                Console.WriteLine(output);
                Console.WriteLine("----error");
                Console.WriteLine(error);
            }
        }

    }
}