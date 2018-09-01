using System.Linq;

namespace Mathematics.Expressions
{
    public class Square : OneArgExpression
    {
        internal override Priority Priority => Priority.Function;

        public Square(Expression arg) : base(arg)
        {
        }

        protected override double _evaluate(double val)
        {
            return val * val;
        }

        public override string ToString()
        {
            return Arg.ToString(Priority) + " ^ 2";
        }

        protected override Expression _derivate()
        {
            return Constant.Two * Arg;
        }

        public override Expression Simplify()
        {
            if (Arg is Hypot)
            {
                return new Add((Arg as Hypot).Args.Select(arg => arg.Square()).ToArray()).Simplify();
            }
            return base.Simplify();
        }
    }
}
