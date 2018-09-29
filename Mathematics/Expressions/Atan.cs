using System;

namespace Mathematics.Expressions
{
    public class Atan : OneArgFunction
    {
        internal Atan(Expression arg) : base("atan", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Atan(value);
        }

        protected override Expression _derivate()
        {
            return Constant.One / (Constant.One + Arg.Square());
        }
    }
}
