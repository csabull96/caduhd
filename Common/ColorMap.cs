using System;

namespace Caduhd.Common
{
    public class ColorMap
    {
        private const double RATIO_UPPER_BOUND_MODIFIER = 0.85;
        private const double RATIO_LOWER_BOUND_MODIFIER = 1.15;

        public DoubleRange Blue { get; private set; }
        public DoubleRange Green { get; private set; }
        public DoubleRange Red { get; private set; }

        public DoubleRange BluePerGreen { get; private set; }
        public DoubleRange BluePerRed { get; private set; }
        public DoubleRange GreenPerRed { get; private set; }

        public ColorMap(IHistogram blue, IHistogram green, IHistogram red)
        {
            Blue = new DoubleRange(blue.Smallest, blue.Greatest);
            Green = new DoubleRange(green.Smallest, green.Greatest);
            Red = new DoubleRange(red.Smallest, red.Greatest);

            double bluePerGreenLowerBound = Blue.LowerBound / Green.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double bluePerGreenUpperBound = Blue.UpperBound / Green.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            BluePerGreen = new DoubleRange(bluePerGreenLowerBound, bluePerGreenUpperBound);

            double bluePerRedLowerBound = Blue.LowerBound / Red.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double bluePerRedUpperBound = Blue.UpperBound / Red.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            BluePerRed = new DoubleRange(bluePerRedLowerBound, bluePerRedUpperBound);

            double greenPerRedLowerBound = Green.LowerBound / Red.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double greenPerRedUpperBound = Green.UpperBound / Red.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            GreenPerRed = new DoubleRange(greenPerRedLowerBound, greenPerRedUpperBound);
        }

        public bool Satisfies(BgrPixel pixel)
        {
            try
            {
                return Blue.IsWithinRange(pixel.Blue) &&
                    Green.IsWithinRange(pixel.Green) &&
                    Red.IsWithinRange(pixel.Red) &&
                    BluePerGreen.IsWithinRange((double)pixel.Blue / pixel.Green) &&
                    BluePerRed.IsWithinRange((double)pixel.Blue / pixel.Red) &&
                    GreenPerRed.IsWithinRange((double)pixel.Green / pixel.Red);
            }
            catch (DivideByZeroException)
            {
                return false;
            }
        }
    }
}
