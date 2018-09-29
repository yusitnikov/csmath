using Mathematics.Math3D;
using System;
using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public struct Point3DExpression
    {
        public Expression X, Y, Z;

        public Expression SquareLength => X * X + Y * Y + Z * Z;
        public Expression Length => Expression.Hypot(X, Y, Z);

        public Point3DExpression Normal
        {
            get
            {
                var result = this / Length;
                result.Alias = alias == null ? null : "Normal(" + alias + ")";
                return result;
            }
        }

        public Expression Pitch
        {
            get
            {
                var result = Expression.Acos(Normal.Y);
                result.Alias = alias == null ? null : "Pitch(" + alias + ")";
                return result;
            }
        }
        public Expression Yaw
        {
            get
            {
                var result = Expression.Atan2(Z, X);
                result.Alias = alias == null ? null : "Yaw(" + alias + ")";
                return result;
            }
        }

        private string alias;
        public string Alias
        {
            get => alias;
            set
            {
                alias = value;
                if (value == null)
                {
                    X.Alias = Y.Alias = Z.Alias = null;
                }
                else
                {
                    X.Alias = value + "X";
                    Y.Alias = value + "Y";
                    Z.Alias = value + "Z";
                }
            }
        }

        public Point3DExpression(Expression x, Expression y, Expression z)
        {
            alias = null;
            X = x;
            Y = y;
            Z = z;
        }

        public Point3DExpression(Point3D point)
        {
            alias = null;
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        public static Point3DExpression FromAngles(Expression pitch, Expression yaw)
        {
            return ((Point3DExpression)Point3D.YAxis).RotatePitch(pitch).RotateYaw(yaw);
        }

        public static Point3DExpression Coalesce(Expression conditionValue, Point3DExpression valueIfNil, Point3DExpression valueIfNotNil)
        {
            return new Point3DExpression(
                Expression.Coalesce(conditionValue, valueIfNil.X, valueIfNotNil.X),
                Expression.Coalesce(conditionValue, valueIfNil.Y, valueIfNotNil.Y),
                Expression.Coalesce(conditionValue, valueIfNil.Z, valueIfNotNil.Z)
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

        public Point3D Evaluate(int cacheGeneration = 0)
        {
            if (cacheGeneration == 0)
            {
                cacheGeneration = Expression.NextAutoIncrementId;
            }
            return new Point3D(X.Evaluate(cacheGeneration), Y.Evaluate(cacheGeneration), Z.Evaluate(cacheGeneration));
        }

        public Point3DExpression EvaluateVars(params Variable[] excludeVariables)
        {
            return new Point3DExpression(
                X.EvaluateVars(excludeVariables),
                Y.EvaluateVars(excludeVariables),
                Z.EvaluateVars(excludeVariables)
            );
        }

        public Point3DExpression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions)
        {
            var cache = new Dictionary<int, Expression>();
            return new Point3DExpression(
                X.SubstituteVariables(cache, substitutions),
                Y.SubstituteVariables(cache, substitutions),
                Z.SubstituteVariables(cache, substitutions)
            );
        }

        public Point3DExpression Derivate(Variable v)
        {
            return new Point3DExpression(
                X.Derivate(v),
                Y.Derivate(v),
                Z.Derivate(v)
            );
        }

        public struct ProjectionToNormalExpression
        {
            public Point3DExpression Vertical, Horizontal;

            public Point3DExpression Full => Vertical + Horizontal;

            public string Alias
            {
                set
                {
                    if (value != null)
                    {
                        Vertical.Alias = value + "_v";
                        Horizontal.Alias = value + "_h";
                    }
                }
            }
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
                Horizontal = this - vertical,
                Alias = Alias
            };
        }

        public ProjectionToNormalExpression ProjectToHorizontalSurface()
        {
            var c0 = Constant.Nil;
            return new ProjectionToNormalExpression()
            {
                Vertical = new Point3DExpression(c0, Y, c0),
                Horizontal = new Point3DExpression(X, c0, Z),
                Alias = Alias
            };
        }

        public Point3DExpression RotateByAngle3D(Point3DExpression angle3D)
        {
            var angleDirection = angle3D.Normal;
            var projection = ProjectToNormalVector(angleDirection);
            var angleLength = angle3D.Length;
            return projection.Vertical + projection.Horizontal * Expression.Cos(angleLength) + VectorMult(this, angleDirection) * Expression.Sin(angleLength);
        }

        // Rotate around OZ: from OY to OX
        public Point3DExpression RotatePitch(Expression angle)
        {
            return RotateByAngle3D(new Point3DExpression(0, 0, angle));
        }

        // Rotate around OY: from OX to OZ
        public Point3DExpression RotateYaw(Expression angle)
        {
            return RotateByAngle3D(new Point3DExpression(0, angle, 0));
        }

        // Rotate around OX: from OZ to OY
        public Point3DExpression RotateRoll(Expression angle)
        {
            return RotateByAngle3D(new Point3DExpression(angle, 0, 0));
        }

        public static Point3DExpression VectorMult(Point3DExpression p1, Point3DExpression p2)
        {
            var result = new Point3DExpression(
                p1.Y * p2.Z - p1.Z * p2.Y,
                p1.Z * p2.X - p1.X * p2.Z,
                p1.X * p2.Y - p1.Y * p2.X
            );
            if (p1.alias != null && p2.alias != null)
            {
                result.Alias = "(" + p1.alias + " x " + p2.alias + ")";
            }
            return result;
        }

        public static Expression ScalarMult(Point3DExpression p1, Point3DExpression p2)
        {
            var result = p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
            if (p1.alias != null && p2.alias != null)
            {
                result.Alias = "dot(" + p1.alias + ", " + p2.alias + ")";
            }
            return result;
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

        public override string ToString()
        {
            return "(" + X + ";" + Y + ";" + Z + ")";
        }
    }
}
