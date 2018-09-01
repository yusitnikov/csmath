using System;

namespace Mathematics.Expressions
{
    public class Sin : OneArgFunction
    {
        public Sin(Expression arg) : base("sin", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Sin(value);
        }

        protected override Expression _derivate()
        {
            return new Cos(Arg);
        }
    }
}
