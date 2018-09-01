namespace Mathematics.Expressions
{
    public class Invert : OneArgExpression
    {
        public Invert(Expression arg) : base(arg)
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

        public override string ToString()
        {
            return "-" + Arg.ToString(Priority);
        }

        protected override Expression _derivate()
        {
            return Constant.MinusOne;
        }
    }
}
