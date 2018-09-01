using System;

namespace Mathematics.Expressions
{
    public class Log : OneArgFunction
    {
        public Log(Expression arg) : base("log", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Log(value);
        }

        protected override Expression _derivate()
        {
            return Constant.One / Arg;
        }

        public override Expression Simplify()
        {
            if (Arg is Exp)
            {
                return (Arg as Exp).Arg.Simplify();
            }
            return base.Simplify();
        }
    }
}
