using System;

namespace Mathematics.Expressions
{
    public class Atan2 : TwoArgsFunction
    {
        public Atan2(Expression y, Expression x) : base("atan2", y, x)
        {
        }

        protected override double _evaluate(double y, double x)
        {
            return Math.Atan2(y, x);
        }

        protected override Expression derivate(Variable variable)
        {
            return (Arg2 * Arg1.Derivate(variable) - Arg1 * Arg2.Derivate(variable)) / (Arg1.Square() + Arg2.Square());
        }

        public override Expression Simplify()
        {
            return this;
        }
    }
}
