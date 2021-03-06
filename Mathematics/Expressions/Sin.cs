using System;

namespace Mathematics.Expressions
{
    public class Sin : OneArgFunction
    {
        internal Sin(Expression arg) : base("sin", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Sin(value);
        }

        protected override Expression _derivate()
        {
            return Cos(Arg);
        }
    }
}
