namespace Mathematics.Expressions
{
    public struct ComplexExpression
    {
        public static ComplexExpression Nil = Complex.Nil, One = Complex.One, I = Complex.I;

        public Expression Re, Im;

        public Expression SquareLength => Re.Square() + Im.Square();
        public Expression Length
        {
            get
            {
                var result = Expression.Hypot(Re, Im);
                result.Alias = alias == null ? null : "Len(" + alias + ")";
                return result;
            }
        }

        public Expression Arg
        {
            get
            {
                var result = Expression.Atan2(Im, Re);
                result.Alias = alias == null ? null : "Arg(" + alias + ")";
                return result;
            }
        }

        public ComplexExpression Normal
        {
            get
            {
                var result = this / Length;
                result.Alias = alias == null ? null : "Normal(" + alias + ")";
                return result;
            }
        }

        public ComplexExpression Conjugate => new ComplexExpression(Re, -Im);

        private string alias;
        public string Alias
        {
            get => alias;
            set
            {
                alias = value;
                Re.Alias = "Re(" + value + ")";
                Im.Alias = "Im(" + value + ")";
            }
        }

        public ComplexExpression(Expression re, Expression im)
        {
            Re = re;
            Im = im;
            alias = null;
        }
        public ComplexExpression(Complex z) : this(z.Re, z.Im) { }

        public static ComplexExpression Coalesce(Expression conditionValue, ComplexExpression valueIfNil, ComplexExpression valueIfNotNil)
        {
            return new ComplexExpression(
                Expression.Coalesce(conditionValue, valueIfNil.Re, valueIfNotNil.Re),
                Expression.Coalesce(conditionValue, valueIfNil.Im, valueIfNotNil.Im)
            );
        }

        public ComplexExpression Derivate(Variable variable)
        {
            return new ComplexExpression(Re.Derivate(variable), Im.Derivate(variable));
        }

        public static ComplexExpression Exp(ComplexExpression z)
        {
            return Expression.Exp(z.Re) * new ComplexExpression(Expression.Cos(z.Im), Expression.Sin(z.Im));
        }

        // int (e^ix / x) dx = (Ci(x); Si(x)) + c
        public static ComplexExpression ExpIntOfImaginaryArg(Expression x)
        {
            return new ComplexExpression(Expression.Ci(x), Expression.Si(x));
        }

        // int from x1 to x2 (e^ix / x) dx
        public static ComplexExpression ExpIntOfImaginaryArg(Expression x1, Expression x2)
        {
            return ExpIntOfImaginaryArg(x2) - ExpIntOfImaginaryArg(x1);
        }

        #region Operators

        public static ComplexExpression operator +(ComplexExpression z1, ComplexExpression z2)
        {
            return new ComplexExpression(z1.Re + z2.Re, z1.Im + z2.Im);
        }

        public static ComplexExpression operator -(ComplexExpression z1, ComplexExpression z2)
        {
            return new ComplexExpression(z1.Re - z2.Re, z1.Im - z2.Im);
        }

        public static ComplexExpression operator -(ComplexExpression z)
        {
            return new ComplexExpression(-z.Re, -z.Im);
        }

        public static ComplexExpression operator *(ComplexExpression z, Expression k)
        {
            return new ComplexExpression(z.Re * k, z.Im * k);
        }

        public static ComplexExpression operator *(Expression k, ComplexExpression z)
        {
            return new ComplexExpression(k * z.Re, k * z.Im);
        }

        public static ComplexExpression operator *(ComplexExpression z1, ComplexExpression z2)
        {
            return new ComplexExpression(z1.Re * z2.Re - z1.Im * z2.Im, z1.Re * z2.Im + z2.Re * z1.Im);
        }

        public static ComplexExpression operator /(ComplexExpression z, Expression k)
        {
            return new ComplexExpression(z.Re / k, z.Im / k);
        }

        public static ComplexExpression operator /(Expression k, ComplexExpression z)
        {
            return z.Conjugate * (k / z.SquareLength);
        }

        public static ComplexExpression operator /(ComplexExpression z1, ComplexExpression z2)
        {
            return new ComplexExpression(z1.Re * z2.Re + z1.Im * z2.Im, z1.Im * z2.Re - z1.Re * z2.Im) / z2.SquareLength;
        }

        public static implicit operator ComplexExpression(Expression x)
        {
            return new ComplexExpression(x, Constant.Nil);
        }

        public static implicit operator ComplexExpression(Complex z)
        {
            return new ComplexExpression(z);
        }

        #endregion
    }
}
