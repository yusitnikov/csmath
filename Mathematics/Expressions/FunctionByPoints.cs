using Mathematics.Math3D;
using System;

namespace Mathematics.Expressions
{
    public class FunctionByPoints : OneArgFunction
    {
        public double[] Values { get; private set; }
        public new double Min { get; private set; }
        public double Step { get; private set; }
        public new double Max => Min + Step * (Values.Length - 1);

        public Expression Derivative { get; set; }

        public FunctionByPoints(string name, Expression arg, double[] values, double min, double step, Expression derivative = null) : base(name, arg)
        {
            Values = values;
            Min = min;
            Step = step;
            Derivative = derivative;
        }

        public static Point3DExpression Create3DFunctionByPoints(string name, Expression arg, Point3D[] values, double min, double step, Point3DExpression? derivative = null)
        {
            var length = values.Length;
            var xValues = new double[length];
            var yValues = new double[length];
            var zValues = new double[length];
            for (var index = 0; index < length; index++)
            {
                xValues[index] = values[index].X;
                yValues[index] = values[index].Y;
                zValues[index] = values[index].Z;
            }

            return new Point3DExpression(
                new FunctionByPoints(name + "X", arg, xValues, min, step, derivative.HasValue ? derivative.Value.X : null).Simplify(),
                new FunctionByPoints(name + "Y", arg, yValues, min, step, derivative.HasValue ? derivative.Value.Y : null).Simplify(),
                new FunctionByPoints(name + "Z", arg, zValues, min, step, derivative.HasValue ? derivative.Value.Z : null).Simplify()
            );
        }

        protected override Expression _derivate()
        {
            if (Derivative == null)
            {
                throw new NotImplementedException("Derivative is not defined");
            }
            else
            {
                return Derivative;
            }
        }

        protected override double _evaluate(double val)
        {
            var doubleIndex = (val - Min) / Step;
            if (double.IsNaN(doubleIndex))
            {
                doubleIndex = 0;
            }
            var intIndex = Math.Min((int)Math.Floor(doubleIndex), Values.Length - 2);
            var coeff = doubleIndex - intIndex;
            return Values[intIndex] * (1 - coeff) + Values[intIndex + 1] * coeff;
        }

        protected override Expression newInstanceWithOtherArgs(params object[] args)
        {
            return new FunctionByPoints(functionName, args[0] as Expression, Values, Min, Step, Derivative).Simplify();
        }
    }
}
