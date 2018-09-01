using System;

namespace Mathematics.Expressions
{
    public class Sqrt : OneArgFunction
    {
        public Sqrt(Expression arg) : base("sqrt", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Sqrt(value);
        }

        protected override Expression _derivate()
        {
            return Constant.Half / this;
        }
    }
}
