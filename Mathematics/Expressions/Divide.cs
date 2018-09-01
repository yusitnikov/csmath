namespace Mathematics.Expressions
{
    public class Divide : TwoArgsExpression
    {
        public Divide(Expression arg1, Expression arg2) : base(arg1, arg2)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Multiply; }
        }

        protected override double _evaluate(double arg1, double arg2)
        {
            return arg2 == 0 ? 0 : arg1 / arg2;
        }

        public override string ToString()
        {
            return Arg1.ToString(Priority) + " / " + Arg2.ToString(Priority);
        }

        protected override Expression derivate(Variable variable)
        {
            return (Arg1.Derivate(variable) * Arg2 - Arg1 * Arg2.Derivate(variable)) / Arg2.Square();
        }

        public override Expression Simplify()
        {
            if (Arg2 is Constant c2)
            {
                return (Arg1 * new Constant(1 / c2.Value)).Simplify();
            }
            if (Arg1 is Constant c1 && c1.Value == 0)
            {
                return Constant.Nil;
            }
            return this;
        }
    }
}
