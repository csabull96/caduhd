using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector.Calibrator
{
    public class ColorCharacteristics
    {
        private const double RATIO_UPPER_BOUND_MODIFIER = 0.85;
        private const double RATIO_LOWER_BOUND_MODIFIER = 1.15;

        public DoubleRange Blue { get; private set; }
        public DoubleRange Green { get; private set; }
        public DoubleRange Red { get; private set; }

        public DoubleRange BluePerGreen { get; private set; }
        public DoubleRange BluePerRed { get; private set; }
        public DoubleRange GreenPerRed { get; private set; }

        public ColorCharacteristics(Histogram blueHistogram, Histogram greenHistogram, Histogram redHistogram)
        {
            Blue = new DoubleRange(blueHistogram.DarkestValue, blueHistogram.BrightestValue);
            Green = new DoubleRange(greenHistogram.DarkestValue, greenHistogram.BrightestValue);
            Red = new DoubleRange(redHistogram.DarkestValue, redHistogram.BrightestValue);

            double bluePerGreenRatioLowerBound = Blue.LowerBound / Green.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double bluePerGreenRatioUpperBound = Blue.UpperBound / Green.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            BluePerGreen = new DoubleRange(bluePerGreenRatioLowerBound, bluePerGreenRatioUpperBound);

            double bluePerRedRatioLowerBound = Blue.LowerBound / Red.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double bluePerRedRatioUpperBound = Blue.UpperBound / Red.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            BluePerRed = new DoubleRange(bluePerRedRatioLowerBound, bluePerRedRatioUpperBound);

            double greenPerRedRatioLowerBound = Green.LowerBound / Red.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double greenPerRedRatioUpperBound = Green.UpperBound / Red.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            GreenPerRed = new DoubleRange(greenPerRedRatioLowerBound, greenPerRedRatioUpperBound);
        }
    }
}
