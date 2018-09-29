using Mathematics.Math3D;
using System;

namespace Mathematics.Expressions
{
    public struct MatrixExpression
    {
        public Expression[,] Elements;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Expression this[int y, int x]
        {
            get => Elements[y, x];
            set => Elements[y, x] = value;
        }

        public MatrixExpression(Expression[,] elements)
        {
            Elements = elements;
            Width = elements.GetLength(1);
            Height = elements.GetLength(0);
        }

        public MatrixExpression(
            Expression a11, Expression a21, Expression a31,
            Expression a12, Expression a22, Expression a32,
            Expression a13, Expression a23, Expression a33
        ) : this(new Expression[3, 3]
        {
            { a11, a21, a31 },
            { a12, a22, a32 },
            { a13, a23, a33 }
        })
        {
        }

        public MatrixExpression(double[,] elements)
        {
            Elements = new Expression[elements.GetLength(0), elements.GetLength(1)];
            Width = elements.GetLength(1);
            Height = elements.GetLength(0);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Elements[y, x] = elements[y, x];
                }
            }
        }

        public static MatrixExpression GetRotateXToYMatrix(Expression angle)
        {
            Expression cos = Expression.Cos(angle), sin = Expression.Sin(angle), c0 = Constant.Nil, c1 = Constant.One;
            return new MatrixExpression(
                cos, -sin, c0,
                sin, cos, c0,
                c0, c0, c1
            );
        }

        public static MatrixExpression GetRotateYToXMatrix(Expression angle)
        {
            Expression cos = Expression.Cos(angle), sin = Expression.Sin(angle), c0 = Constant.Nil, c1 = Constant.One;
            return new MatrixExpression(
                cos, sin, c0,
                -sin, cos, c0,
                c0, c0, c1
            );
        }

        public static MatrixExpression GetRotateYToZMatrix(Expression angle)
        {
            Expression cos = Expression.Cos(angle), sin = Expression.Sin(angle), c0 = Constant.Nil, c1 = Constant.One;
            return new MatrixExpression(
                c1, c0, c0,
                c0, cos, -sin,
                c0, sin, cos
            );
        }

        public static MatrixExpression GetRotateZToYMatrix(Expression angle)
        {
            Expression cos = Expression.Cos(angle), sin = Expression.Sin(angle), c0 = Constant.Nil, c1 = Constant.One;
            return new MatrixExpression(
                c1, c0, c0,
                c0, cos, sin,
                c0, -sin, cos
            );
        }

        public static MatrixExpression GetRotateXToZMatrix(Expression angle)
        {
            Expression cos = Expression.Cos(angle), sin = Expression.Sin(angle), c0 = Constant.Nil, c1 = Constant.One;
            return new MatrixExpression(
                cos, c0, -sin,
                c0, c1, c0,
                sin, c0, cos
            );
        }

        public static MatrixExpression GetRotateZToXMatrix(Expression angle)
        {
            Expression cos = Expression.Cos(angle), sin = Expression.Sin(angle), c0 = Constant.Nil, c1 = Constant.One;
            return new MatrixExpression(
                cos, c0, sin,
                c0, c1, c0,
                -sin, c0, cos
            );
        }

        public Matrix Evaluate(int cacheGeneration = 0)
        {
            if (cacheGeneration == 0)
            {
                cacheGeneration = Expression.NextAutoIncrementId;
            }
            var values = new double[Height, Width];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    values[y, x] = Elements[y, x].Evaluate(cacheGeneration);
                }
            }
            return new Matrix(values);
        }

        public static MatrixExpression operator *(MatrixExpression m1, MatrixExpression m2)
        {
            if (m1.Width != m2.Height)
            {
                throw new InvalidOperationException("Trying to multiply matrixes of invalid sizes");
            }

            var r = new MatrixExpression(new Expression[m1.Height, m2.Width]);
            for (int y = 0; y < m1.Height; y++)
            {
                for (int x = 0; x < m2.Width; x++)
                {
                    var args = new Expression[m1.Width];
                    for (int k = 0; k < m1.Width; k++)
                    {
                        args[k] = m1[y, k] * m2[k, x];
                    }
                    r[y, x] = Expression.Sum(args);
                }
            }
            return r;
        }

        public static Point3DExpression operator *(MatrixExpression m, Point3DExpression v)
        {
            return (m * v.ToVerticalMatrix()).ToVector();
        }

        [Obsolete("Multiplying vector to matrix is deprecated. Try to multiply matrix to vector. Order is important!", true)]
        public static Point3DExpression operator *(Point3DExpression v, MatrixExpression m)
        {
            return (v.ToHorizontalMatrix() * m).ToVector();
        }

        public Point3DExpression ToVector()
        {
            if (Width == 1 && Height == 3)
            {
                return new Point3DExpression(Elements[0, 0], Elements[1, 0], Elements[2, 0]);
            }
            else if (Width == 3 && Height == 1)
            {
                return new Point3DExpression(Elements[0, 0], Elements[0, 1], Elements[0, 2]);
            }
            else
            {
                throw new InvalidOperationException("Trying to convert matrix of invalid size to point3d");
            }
        }
    }
}
