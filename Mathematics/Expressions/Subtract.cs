namespace Mathematics.Expressions
{
    public class Subtract : TwoArgsExpression
    {
        internal Subtract(Expression arg1, Expression arg2) : base(arg1, arg2)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Add; }
        }

        protected override double _evaluate(double arg1, double arg2)
        {
            return arg1 - arg2;
        }

        protected override string toString(int depth)
        {
            return Arg1.ToString(depth, Priority) + " - " + Arg2.ToString(depth, Priority);
        }

        protected override Expression derivate(Variable variable)
        {
            return Arg1.Derivate(variable) - Arg2.Derivate(variable);
        }
    }
}
