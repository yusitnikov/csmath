namespace Mathematics.Expressions
{
    public class Invert : OneArgExpression
    {
        internal Invert(Expression arg) : base(arg)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Add; }
        }

        protected override double _evaluate(double value)
        {
            return -value;
        }

        protected override string toString(int depth)
        {
            return "-" + Arg.ToString(depth, Priority);
        }

        protected override Expression _derivate()
        {
            return Constant.MinusOne;
        }
    }
}
