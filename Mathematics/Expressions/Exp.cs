using System;

namespace Mathematics.Expressions
{
    public class Exp : OneArgExpression
    {
        internal override Priority Priority => Priority.Function;

        internal Exp(Expression arg) : base(arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Exp(value);
        }

        protected override string toString(int depth)
        {
            return "e ^ " + Arg.ToString(depth, Priority);
        }

        protected override Expression _derivate()
        {
            return this;
        }

        internal override Expression Simplify()
        {
            if (Arg is Log log)
            {
                return log.Arg;
            }
            return base.Simplify();
        }
    }
}
