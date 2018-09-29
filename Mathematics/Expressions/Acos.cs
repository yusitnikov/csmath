using System;

namespace Mathematics.Expressions
{
    public class Acos : OneArgFunction
    {
        internal Acos(Expression arg) : base("acos", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Acos(value);
        }

        protected override Expression _derivate()
        {
            return Constant.MinusOne / Sqrt(Constant.One - Arg.Square());
        }
    }
}
