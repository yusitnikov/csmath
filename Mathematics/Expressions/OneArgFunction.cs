namespace Mathematics.Expressions
{
    public abstract class OneArgFunction : OneArgExpression
    {
        protected readonly string functionName;

        internal override Priority Priority
        {
            get { return Priority.Function; }
        }

        public OneArgFunction(string name, Expression arg) : base(arg)
        {
            functionName = name;
        }

        protected override string toString(int depth)
        {
            return functionName + "(" + Arg.ToString(depth) + ")";
        }
    }
}
