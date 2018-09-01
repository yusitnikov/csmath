namespace Mathematics.Expressions
{
    public class Subtract : TwoArgsExpression
    {
        public Subtract(Expression arg1, Expression arg2) : base(arg1, arg2)
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

        public override string ToString()
        {
            return Arg1.ToString(Priority) + " - " + Arg2.ToString(Priority);
        }

        protected override Expression derivate(Variable variable)
        {
            return Arg1.Derivate(variable) - Arg2.Derivate(variable);
        }

        public override Expression Simplify()
        {
            return (Arg1 + (-Arg2)).Simplify();
        }
    }
}
