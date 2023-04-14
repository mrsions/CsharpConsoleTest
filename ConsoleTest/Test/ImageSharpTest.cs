using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Test
{
    public class ImageSharpTest
    {
        public async Task Run()
        {
            string src = @"D:\Work\Graphic\00.Company\리얼위드CI\logo_h_4096.jpg";
            await NewMethod(src, @"D:\a.jpg", 2048, 2048);
            await NewMethod(src, @"D:\b.jpg", 2048, 256);
            await NewMethod(src, @"D:\c.jpg", 10240, 256);
        }

        private static async Task NewMethod(string src, string dst, int w , int h)
        {
            using (Stream readStream = File.OpenRead(src))
            using (Stream writeStream = File.OpenWrite(dst))
            using (Image img = await Image.LoadAsync(readStream))
            {
                img.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(w, h)
                }));

                await img.SaveAsync(writeStream, new PngEncoder());
            }
        }
    }
}
