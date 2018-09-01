using Mathematics.Math3D;
using System;

namespace Mathematics.Expressions
{
    public struct Point3DExpression
    {
        public Expression X, Y, Z;

        public Expression SquareLength => X * X + Y * Y + Z * Z;
        public Expression Length => new Hypot(X, Y, Z);

        public Point3DExpression Normal => this / Length;

        public Point3DExpression(Expression x, Expression y, Expression z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3DExpression(Point3D point)
        {
            X = new Constant(point.X);
            Y = new Constant(point.Y);
            Z = new Constant(point.Z);
        }

        public static Point3DExpression Coalesce(Expression conditionValue, Point3DExpression valueIfNil, Point3DExpression valueIfNotNil)
        {
            return new Point3DExpression(
                new Coalesce(conditionValue, valueIfNil.X, valueIfNotNil.X),
                new Coalesce(conditionValue, valueIfNil.Y, valueIfNotNil.Y),
                new Coalesce(conditionValue, valueIfNil.Z, valueIfNotNil.Z)
            );
        }

        public Point3DExpression Each(Converter<Expression, Expression> converter)
        {
            return new Point3DExpression(
                converter(X),
                converter(Y),
                converter(Z)
            );
        }

        public T[] Each<T>(Converter<Expression, T> converter)
        {
            return new T[]
            {
                converter(X),
                converter(Y),
                converter(Z)
            };
        }

        public Point3D Each(Converter<Expression, double> converter)
        {
            return new Point3D(
                converter(X),
                converter(Y),
                converter(Z)
            );
        }

        public Point3D Evaluate()
        {
            return Each(v => v.Evaluate());
        }

        public Point3DExpression EvaluateVars(params Variable[] excludeVariables)
        {
            return Each(v => v.EvaluateVars(excludeVariables));
        }

        public Point3DExpression Simplify()
        {
            return Each(v => v.Simplify());
        }

        public struct ProjectionToNormalExpression
        {
            public Point3DExpression Vertical, Horizontal;

            public Point3DExpression Full => Vertical + Horizontal;
        }

        public Expression GetProjectionToNormalVectorLength(Point3DExpression normal)
        {
            return ScalarMult(this, normal);
        }

        public ProjectionToNormalExpression ProjectToNormalVector(Point3DExpression normal)
        {
            var vertical = GetProjectionToNormalVectorLength(normal) * normal;
            return new ProjectionToNormalExpression()
            {
                Vertical = vertical,
                Horizontal = this - vertical
            };
        }

        public Point3DExpression RotateByAngle3D(Point3DExpression angle3D)
        {
            var angleDirection = angle3D.Normal;
            var projection = ProjectToNormalVector(angleDirection);
            var angleLength = angle3D.Length;
            return projection.Vertical + projection.Horizontal * new Cos(angleLength) + VectorMult(this, angleDirection) * new Sin(angleLength);
        }

        public static Point3DExpression VectorMult(Point3DExpression p1, Point3DExpression p2)
        {
            return new Point3DExpression(
                p1.Y * p2.Z - p1.Z * p2.Y,
                p1.Z * p2.X - p1.X * p2.Z,
                p1.X * p2.Y - p1.Y * p2.X
            );
        }

        public static Expression ScalarMult(Point3DExpression p1, Point3DExpression p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }

        #region Operators

        public static Point3DExpression operator +(Point3DExpression p1, Point3DExpression p2)
        {
            return new Point3DExpression(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static Point3DExpression operator -(Point3DExpression p1, Point3DExpression p2)
        {
            return new Point3DExpression(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point3DExpression operator -(Point3DExpression p)
        {
            return new Point3DExpression(-p.X, -p.Y, -p.Z);
        }

        public static Point3DExpression operator *(Point3DExpression p, Expression k)
        {
            return new Point3DExpression(p.X * k, p.Y * k, p.Z * k);
        }

        public static Point3DExpression operator *(Expression k, Point3DExpression p)
        {
            return new Point3DExpression(k * p.X, k * p.Y, k * p.Z);
        }

        public static Point3DExpression operator /(Point3DExpression p, Expression k)
        {
            return new Point3DExpression(p.X / k, p.Y / k, p.Z / k);
        }

        public static implicit operator Point3DExpression(Point3D p)
        {
            return new Point3DExpression(p);
        }

        #endregion

        public MatrixExpression ToHorizontalMatrix()
        {
            return new MatrixExpression(new Expression[1, 3] { { X, Y, Z } });
        }

        public MatrixExpression ToVerticalMatrix()
        {
            return new MatrixExpression(new Expression[3, 1] { { X }, { Y }, { Z } });
        }
    }
}
