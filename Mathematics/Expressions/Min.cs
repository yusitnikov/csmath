using System;

namespace Mathematics.Expressions
{
    public class Min : TwoArgsFunction
    {
        internal Min(Expression expression1, Expression expression2) : base("min", expression1, expression2)
        {
        }

        protected override double _evaluate(double value1, double value2)
        {
            return Math.Min(value1, value2);
        }

        protected override Expression derivate(Variable variable)
        {
            return IfPositive(Arg1 - Arg2, Arg2.Derivate(variable), Arg1.Derivate(variable));
        }
    }
}
