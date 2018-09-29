using System;

namespace Mathematics.Expressions
{
    public class Cos : OneArgFunction
    {
        internal Cos(Expression arg) : base("cos", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Cos(value);
        }

        protected override Expression _derivate()
        {
            return -Sin(Arg);
        }
    }
}
