namespace Mathematics.Expressions
{
    public abstract class TwoArgsFunction : TwoArgsExpression
    {
        private readonly string functionName;

        internal override Priority Priority
        {
            get { return Priority.Function; }
        }

        public TwoArgsFunction(string name, Expression arg1, Expression arg2) : base(arg1, arg2)
        {
            functionName = name;
        }

        protected override string toString(int depth)
        {
            return functionName + "(" + Arg1.ToString(depth) + "; " + Arg2.ToString(depth) + ")";
        }
    }
}
