using System;
using System.Collections.Generic;

namespace Mathematics.Math3D
{
    public class Matrix
    {
        #region Pre-allocated memory
        private static List<double[,]> temporarySquareArrays = new List<double[,]>();
        private static double[,] getTemporarySquareArray(int n)
        {
            lock (temporarySquareArrays)
            {
                for (int i = temporarySquareArrays.Count; i <= n; i++)
                {
                    temporarySquareArrays.Add(new double[i, i]);
                }
                return temporarySquareArrays[n];
            }
        }
        #endregion

        public double[,] Elements;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public double this[int y, int x]
        {
            get => Elements[y, x];
            set => Elements[y, x] = value;
        }

        public Matrix(double[,] elements)
        {
            Elements = elements;
            Width = elements.GetLength(1);
            Height = elements.GetLength(0);
        }

        public Matrix GetMinor(int x, int y)
        {
            if (Width != Height)
            {
                throw new InvalidOperationException("Trying to calculate minor of non-square matrix");
            }
            int n = Width;
            var result = getTemporarySquareArray(n - 1);
            for (int x1 = 0; x1 < n - 1; x1++)
            {
                int x2 = x1 + (x1 >= x ? 1 : 0);
                for (int y1 = 0; y1 < n - 1; y1++)
                {
                    int y2 = y1 + (y1 >= y ? 1 : 0);
                    result[y1, x1] = Elements[y2, x2];
                }
            }
            return new Matrix(result);
        }

        public double GetComplement(int x, int y)
        {
            // (-1) ^ (x + y)
            var sign = (x + y) % 2 == 0 ? 1 : -1;
            return sign * GetMinor(x, y).GetDeterminant();
        }

        public double GetDeterminant()
        {
            if (Width != Height)
            {
                throw new InvalidOperationException("Trying to calculate determinant of non-square matrix");
            }
            int n = Width;

            switch (n)
            {
                case 0:
                    return 0;
                case 1:
                    return Elements[0, 0];
                default:
                    double result = 0;
                    for (int x = 0; x < n; x++)
                    {
                        result += Elements[0, x] * GetComplement(x, 0);
                    }
                    return result;
            }
        }

        public Matrix Invert()
        {
            if (Width != Height)
            {
                throw new InvalidOperationException("Trying to calculate invert of non-square matrix");
            }
            int n = Width;

            var det = GetDeterminant();
            if (det == 0)
            {
                throw new Exception("Determinant is 0 at Matrix.Invert!");
            }
            var result = new double[n, n];
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    result[y, x] = GetComplement(y, x) / det;
                }
            }
            return new Matrix(result);
        }
    }
}
