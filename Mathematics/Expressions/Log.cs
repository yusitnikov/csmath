using System;

namespace Mathematics.Expressions
{
    public class Log : OneArgFunction
    {
        internal Log(Expression arg) : base("log", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return double.IsNaN(value) || value <= 0 ? double.NaN : Math.Log(value);
        }

        protected override Expression _derivate()
        {
            return Constant.One / Arg;
        }

        internal override Expression Simplify()
        {
            if (Arg is Exp exp)
            {
                return exp.Arg;
            }
            return base.Simplify();
        }
    }
}
