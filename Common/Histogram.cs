namespace Caduhd.Common
{
    using System;

    /// <summary>
    /// A histogram.
    /// </summary>
    public class Histogram : IHistogram
    {
        private readonly double min;
        private readonly double max;
        private readonly int numberOfGroups;

        private readonly int[] histogram;
        private int weight = 0;

        /// <summary>
        /// Gets the smallest value in the histogram that is greater than the specified minimum.
        /// If there is no such element, then the minimum value is returned.
        /// </summary>
        public double Smallest
        {
            get
            {
                for (int i = 0; i < this.numberOfGroups; i++)
                {
                    if (0 < this.histogram[i])
                    {
                        return this.min + i * (this.max - this.min) / this.numberOfGroups;
                    }
                }

                return this.min;
            }
        }

        /// <summary>
        /// Gets the greatest value in the histogram that is smaller than the specified maximum.
        /// If there is no such element, then the maximum value is returned.
        /// </summary>
        public double Greatest
        {
            get
            {
                for (int i = this.numberOfGroups - 1; 0 <= i; i--)
                {
                    if (0 < this.histogram[i])
                    {
                        return this.max - (this.numberOfGroups - (i + 1)) * (this.max - this.min) / this.numberOfGroups;
                    }
                }

                return this.max;
            }
        }

        /// <summary>
        /// Gets the normalized underlying histogram. 
        /// </summary>
        public double[] Normalized
        {
            get
            {
                double[] normalized = new double[this.numberOfGroups];

                for (int i = 0; i < this.numberOfGroups; i++)
                {
                    normalized[i] = (double)this.histogram[i] / this.weight;
                }

                return normalized;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Histogram"/> class.
        /// </summary>
        /// <param name="min">The minimum value of the histogram.</param>
        /// <param name="max">The maximum value of the histogram.</param>
        /// <param name="numberOfGroups">The desired number of groups.</param>
        public Histogram(double min, double max, int numberOfGroups)
        {
            if (max <= min)
            {
                throw new ArgumentException("The maximum value has to be greater than the minimum value.");
            }

            if (numberOfGroups < 1)
            {
                throw new ArgumentException("Number of groups has to be greater than zero.");
            }

            this.min = min;
            this.max = max;
            this.numberOfGroups = numberOfGroups;

            this.histogram = new int[this.numberOfGroups];
        }

        /// <summary>
        /// Populates the histogram with the provided value.
        /// </summary>
        /// <param name="value">The value to populate the histogram with.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is smaller than the minimum value or greater than the maximum value.</exception>
        public void Insert(double value)
        {
            if (!this.TryInsert(value))
            {
                throw new ArgumentException("The inserted value has to be greater (or equal) than the minimum value and smaller (or equal) then the maximum value.");
            }
        }

        /// <summary>
        /// Tries to populate the histogram with the provided value.
        /// </summary>
        /// <param name="value">The value to populate the histogram with.</param>
        /// <returns>True if population was successful (the <paramref name="value"/> inclusively falls between the boundaries of the minimum and maximum value), false otherwise.</returns>
        public bool TryInsert(double value)
        {
            if (this.min <= value && value <= this.max)
            {
                int index = (int)((value - this.min) / ((this.max - this.min) / this.numberOfGroups));
                if (index == this.numberOfGroups)
                {
                    index--;
                }

                this.histogram[index]++;
                this.weight++;
                return true;
            }

            return false;
        }
    }
}
