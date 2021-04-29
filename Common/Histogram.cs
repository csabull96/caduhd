using System;

namespace Caduhd.Common
{
    public class Histogram
    {
        private readonly double _min;
        private readonly double _max;
        private readonly int _numberOfGroups;

        private readonly int[] _histogram; 
        private int _weight = 0;

        public double Smallest
        {
            get
            {
                for (int i = 0; i < _numberOfGroups; i++)
                {
                    if (0 < _histogram[i])
                    {
                        return _min + i * (_max - _min) / _numberOfGroups;
                    }
                }

                return _min;
            }
        }

        public double Greatest
        {
            get
            {
                for (int i = _numberOfGroups - 1; 0 <= i; i--)
                {
                    if (0 < _histogram[i])
                    {
                        return _max - (_numberOfGroups - (i + 1)) * (_max - _min) / _numberOfGroups;
                    }
                }

                return _max;
            }
        }

        public double[] Normalized
        {
            get
            {
                double[] normalized = new double[_numberOfGroups];

                for (int i = 0; i < _numberOfGroups; i++)
                {
                    normalized[i] = (double)_histogram[i] / _weight;
                }

                return normalized;
            }
        }

        public Histogram(double min, double max, int numberOfGroups)
        {
            if (max <= min)
                throw new ArgumentException("The maximum value has to be greater than the minimum value.");

            if (numberOfGroups < 1)
                throw new ArgumentException("Number of groups has to be greater than zero.");

            _min = min;
            _max = max;
            _numberOfGroups = numberOfGroups;

            _histogram = new int[_numberOfGroups];
        }

        public void Insert(double value)
        {
            if (!TryInsert(value))
                throw new ArgumentException("The inserted value has to be greater (or equal) than the minimum value and smaller (or equal) then the maximum value.");
        }

        public bool TryInsert(double value)
        {
            if (_min <= value && value <= _max)
            {
                int index = (int)((value - _min) / ((_max - _min) / _numberOfGroups));
                if (index == _numberOfGroups)
                {
                    index--;
                }
                _histogram[index]++;
                _weight++;
                return true;
            }
            return false;
        }
    }
}
