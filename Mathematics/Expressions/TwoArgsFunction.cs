using System;

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

        public override string ToString()
        {
            return functionName + "(" + Arg1 + "; " + Arg2 + ")";
        }
    }
}
