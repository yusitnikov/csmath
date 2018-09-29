namespace Mathematics.Expressions
{
    public class Divide : TwoArgsExpression
    {
        internal Divide(Expression arg1, Expression arg2) : base(arg1, arg2)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Multiply; }
        }

        protected override double evaluate(int cacheGeneration)
        {
            var value1 = Arg1.Evaluate(cacheGeneration);
            return value1 == 0 ? 0 : _evaluate(value1, Arg2.Evaluate(cacheGeneration));
        }
        protected override double _evaluate(double arg1, double arg2)
        {
            return arg2 == 0 ? 0 : arg1 / arg2;
        }

        protected override string toString(int depth)
        {
            return Arg1.ToString(depth, Priority) + " / " + Arg2.ToString(depth, Priority);
        }

        protected override Expression derivate(Variable variable)
        {
            return (Arg1.Derivate(variable) * Arg2 - Arg1 * Arg2.Derivate(variable)) / Arg2.Square();
        }

        internal override Expression Simplify()
        {
            if (Arg1 is Constant c1 && c1.Value == 0 || Arg2 is Constant c2 && c2.Value == 0)
            {
                return Constant.Nil;
            }
            return base.Simplify();
        }
    }
}
