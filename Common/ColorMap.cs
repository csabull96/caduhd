namespace Caduhd.Common
{
    using System;

    /// <summary>
    /// A class that is meant to represent the characterisctic of a certain color.
    /// </summary>
    public class ColorMap
    {
        private const double RATIO_UPPER_BOUND_MODIFIER = 0.85;
        private const double RATIO_LOWER_BOUND_MODIFIER = 1.15;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorMap"/> class.
        /// </summary>
        /// <param name="blue">The histogram generated from blue color channel values.</param>
        /// <param name="green">The histogram generated from green color channel values.</param>
        /// <param name="red">The histogram generated from red color channel values.</param>
        public ColorMap(IHistogram blue, IHistogram green, IHistogram red)
        {
            this.Blue = new DoubleRange(blue.Smallest, blue.Greatest);
            this.Green = new DoubleRange(green.Smallest, green.Greatest);
            this.Red = new DoubleRange(red.Smallest, red.Greatest);

            double bluePerGreenLowerBound = this.Blue.LowerBound / this.Green.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double bluePerGreenUpperBound = this.Blue.UpperBound / this.Green.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            this.BluePerGreen = new DoubleRange(bluePerGreenLowerBound, bluePerGreenUpperBound);

            double bluePerRedLowerBound = this.Blue.LowerBound / this.Red.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double bluePerRedUpperBound = this.Blue.UpperBound / this.Red.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            this.BluePerRed = new DoubleRange(bluePerRedLowerBound, bluePerRedUpperBound);

            double greenPerRedLowerBound = this.Green.LowerBound / this.Red.UpperBound * RATIO_LOWER_BOUND_MODIFIER;
            double greenPerRedUpperBound = this.Green.UpperBound / this.Red.LowerBound * RATIO_UPPER_BOUND_MODIFIER;
            this.GreenPerRed = new DoubleRange(greenPerRedLowerBound, greenPerRedUpperBound);
        }

        /// <summary>
        /// Gets the range that represents the possible values of the blue color channel.
        /// </summary>
        public DoubleRange Blue { get; private set; }

        /// <summary>
        /// Gets the range that represents the possible values of the green color channel.
        /// </summary>
        public DoubleRange Green { get; private set; }

        /// <summary>
        /// Gets the range that represents the possible values of the red color channel.
        /// </summary>
        public DoubleRange Red { get; private set; }

        /// <summary>
        /// Gets the range that represents the possible values of the ratio of the blue color channel to the green color channel.
        /// </summary>
        public DoubleRange BluePerGreen { get; private set; }

        /// <summary>
        /// Gets the range that represents the possible values of the ratio of the blue color channel to the red color channel.
        /// </summary>
        public DoubleRange BluePerRed { get; private set; }

        /// <summary>
        /// Gets the range that represents the possible values of the ratio of the green color channel to the red color channel.
        /// </summary>
        public DoubleRange GreenPerRed { get; private set; }

        /// <summary>
        /// Evaluated a BgrPixel whether it satisfies the color map or not.
        /// </summary>
        /// <param name="pixel">The BgrPixel to evaluate.</param>
        /// <returns>True if BgrPixel satisfies the color map, false otherwise.</returns>
        public bool Satisfies(BgrPixel pixel)
        {
            try
            {
                return this.Blue.IsWithinRange(pixel.Blue) &&
                    this.Green.IsWithinRange(pixel.Green) &&
                    this.Red.IsWithinRange(pixel.Red) &&
                    this.BluePerGreen.IsWithinRange((double)pixel.Blue / pixel.Green) &&
                    this.BluePerRed.IsWithinRange((double)pixel.Blue / pixel.Red) &&
                    this.GreenPerRed.IsWithinRange((double)pixel.Green / pixel.Red);
            }
            catch (DivideByZeroException)
            {
                return false;
            }
        }
    }
}
