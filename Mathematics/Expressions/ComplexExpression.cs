using System.Linq;

namespace Mathematics.Expressions
{
    public struct ComplexExpression
    {
        public static ComplexExpression Nil = Complex.Nil, One = Complex.One, I = Complex.I;

        public Expression Re, Im;

        public Expression SquareLength => new Square(Re) + new Square(Im);
        public Expression Length => new Hypot(Re, Im);

        public Expression Arg => new Atan2(Im, Re);

        public ComplexExpression Normal => this / Length;

        public ComplexExpression Conjugate => new ComplexExpression(Re, -Im);

        public ComplexExpression(Expression re, Expression im)
        {
            Re = re;
            Im = im;
        }
        public ComplexExpression(Complex z) : this(new Constant(z.Re), new Constant(z.Im)) { }

        public static ComplexExpression Coalesce(Expression conditionValue, ComplexExpression valueIfNil, ComplexExpression valueIfNotNil)
        {
            return new ComplexExpression(
                new Coalesce(conditionValue, valueIfNil.Re, valueIfNotNil.Re),
                new Coalesce(conditionValue, valueIfNil.Im, valueIfNotNil.Im)
            );
        }

        public ComplexExpression Derivate(Variable variable)
        {
            return new ComplexExpression(Re.Derivate(variable), Im.Derivate(variable));
        }

        public ComplexExpression Simplify()
        {
            return new ComplexExpression(Re.Simplify(), Im.Simplify());
        }

        public static ComplexExpression Exp(ComplexExpression z)
        {
            return new Exp(z.Re) * new ComplexExpression(new Cos(z.Im), new Sin(z.Im));
        }

        // int (e^ix / x) dx = (Ci(x); Si(x)) + c
        public static ComplexExpression ExpIntOfImaginaryArg(Expression x)
        {
            return new ComplexExpression(new Ci(x), new Si(x));
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
