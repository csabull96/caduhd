﻿using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector.Calibrator
{
    public class ColorCalibrator
    {
        public ColorCharacteristics CreateColorCharacteristics(Image<Bgr, byte> sample, Rectangle mask)
        {
            var blues = new Histogram(sample.Width, sample.Height);
            var greens = new Histogram(sample.Width, sample.Height);
            var reds = new Histogram(sample.Width, sample.Height);

            int xStart = mask.X;
            int xEnd = xStart + mask.Width;

            int yStart = mask.Y;
            int yEnd = yStart + mask.Height;

            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    blues.InsertValue(sample.Data[y, x, 0]);
                    greens.InsertValue(sample.Data[y, x, 1]);
                    reds.InsertValue(sample.Data[y, x, 2]);
                }
            }

            return new ColorCharacteristics(blues, greens, reds);
        }

        public ColorCharacteristics ExtractColorCharacteristics(Image<Bgr, byte> sample, Rectangle mask) 
            => CreateColorCharacteristics(sample, mask);
    }
}