using System;

namespace Mathematics
{
    public class BinarySearch
    {
        public static double Search(double min, double max, double precision, Func<double, bool> comparer, Func<double, double> calculator)
        {
            if (comparer(min))
            {
                return double.NaN;
            }

            var iterations = 0;
            while (!comparer(max))
            {
                min = max;
                max *= 2;
                if (++iterations >= 10)
                {
                    return double.NaN;
                }
            }

            while (max - min > precision)
            {
                var mid = (min + max) / 2;
                if (comparer(mid))
                {
                    max = mid;
                }
                else
                {
                    min = mid;
                }
                ++iterations;
            }

            double v1 = calculator(min), v2 = calculator(max);
            return v1 == v2 ? (min + max) / 2 : Math.Max(min, Math.Min(max, min - (max - min) * v1 / (v2 - v1)));
        }
    }
}
