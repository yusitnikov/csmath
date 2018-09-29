using System;

namespace Mathematics.Expressions
{
    public class Max : TwoArgsFunction
    {
        internal Max(Expression expression1, Expression expression2) : base("max", expression1, expression2)
        {
        }

        protected override double _evaluate(double value1, double value2)
        {
            return Math.Max(value1, value2);
        }

        protected override Expression derivate(Variable variable)
        {
            return IfPositive(Arg1 - Arg2, Arg1.Derivate(variable), Arg2.Derivate(variable));
        }
    }
}
