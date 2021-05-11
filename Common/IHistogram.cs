namespace Caduhd.Common
{
    /// <summary>
    /// IHistogram interface.
    /// </summary>
    public interface IHistogram
    {
        /// <summary>
        /// Gets the smallest value in the histogram.
        /// </summary>
        double Smallest { get; }

        /// <summary>
        /// Gets the greatest value in the histogram.
        /// </summary>
        double Greatest { get; }

        /// <summary>
        /// Gets the normalized underlying histogram.
        /// </summary>
        double[] Normalized { get; }

        /// <summary>
        /// Populates the histogram with the provided value.
        /// </summary>
        /// <param name="value">The value to populate with.</param>
        void Insert(double value);

        /// <summary>
        /// Tries to populate the histogram with the provided value.
        /// </summary>
        /// <param name="value">The value to populate with.</param>
        /// <returns>True if population was successful, false otherwise.</returns>
        bool TryInsert(double value);
    }
}
