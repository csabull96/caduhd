namespace Ksvydo.HandDetector.Calibrator
{
    public class Histogram
    {
        private const int MIN = 0;
        private const int MAX = 255;

        private readonly int m_numberOfPixels;
        private readonly int m_rangeWidth;
        private readonly int[] m_histogram;

        public int DarkestValue
        {
            get
            {
                for (int i = 0; i < m_histogram.Length; i++)
                {
                    if (MIN < m_histogram[i])
                    {
                        return i * m_rangeWidth;
                    }
                }

                return MIN;
            }
        }

        public int BrightestValue
        {
            get
            {

                for (int i = m_histogram.Length - 1; 0 <= i; i--)
                {
                    if (MIN < m_histogram[i])
                    {
                        return (i + 1) * m_rangeWidth - 1;
                    }
                }

                return MAX;
            }
        }

        public double[] Normalized
        {
            get
            {
                double[] normalized = new double[m_histogram.Length];

                for (int i = 0; i < m_histogram.Length; i++)
                {
                    normalized[i] = (double)m_histogram[i] / m_numberOfPixels;
                }

                return normalized;
            }      
        }

        public Histogram(int imageWidth, int imageHeight)
        {
            m_numberOfPixels = imageWidth * imageHeight;
            // be careful here, the following has to be true here: 256 % m_rangeWidth == 0
            m_rangeWidth = 4;
            m_histogram = new int[(MAX + 1) / m_rangeWidth];
        }

        public void InsertValue(int value)
        {
            if (MIN <= value && value <= MAX)
            {
                m_histogram[value / m_rangeWidth]++;
            }
        }
    }
}
