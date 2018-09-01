using System;

namespace Mathematics.Expressions
{
    public class Exp : OneArgExpression
    {
        internal override Priority Priority => Priority.Function;

        public Exp(Expression arg) : base(arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Math.Exp(value);
        }

        public override string ToString()
        {
            return "e ^ " + Arg.ToString(Priority);
        }

        protected override Expression _derivate()
        {
            return this;
        }

        public override Expression Simplify()
        {
            if (Arg is Log)
            {
                return (Arg as Log).Arg.Simplify();
            }
            return base.Simplify();
        }
    }
}
