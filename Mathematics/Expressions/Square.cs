namespace Mathematics.Expressions
{
    public class Square : OneArgExpression
    {
        internal override Priority Priority => Priority.Function;

        internal Square(Expression arg) : base(arg)
        {
        }

        protected override double _evaluate(double val)
        {
            return val * val;
        }

        protected override string toString(int depth)
        {
            return Arg.ToString(depth, Priority) + " ^ 2";
        }

        protected override Expression _derivate()
        {
            return Constant.Two * Arg;
        }

        internal override Expression Simplify()
        {
            if (Arg is Hypot hypot)
            {
                var args = new Expression[hypot.Args.Length];
                for (var i = 0; i < hypot.Args.Length; i++)
                {
                    args[i] = hypot.Args[i].Square();
                }
                return Sum(args);
            }
            return base.Simplify();
        }
    }
}
