using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest.Test
{
    public class ColorSpread
    {
        public ColorSpread(string[] args)
        {
            var amg = Image.Load<Rgba32>("d:/a.png");
            var bmg = Image.Load<Rgba32>("d:/b.png");

            
        }
    }
}
