using System;

namespace Mathematics.Expressions
{
    public abstract class OneArgFunction : OneArgExpression
    {
        private readonly string functionName;

        internal override Priority Priority
        {
            get { return Priority.Function; }
        }

        public OneArgFunction(string name, Expression arg) : base(arg)
        {
            functionName = name;
        }

        public override string ToString()
        {
            return functionName + "(" + Arg + ")";
        }
    }
}
