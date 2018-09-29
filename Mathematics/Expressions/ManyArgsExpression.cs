using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public abstract class ManyArgsExpression : Expression
    {
        protected override Expression[] arguments
        {
            get => Args;
            set => Args = value;
        }

        public Expression[] Args { get; private set; }

        public ManyArgsExpression(params Expression[] args)
        {
            Args = new Expression[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                Args[i] = args[i];
            }
        }

        protected abstract string separator { get; }

        protected override string toString(int depth)
        {
            string s = Args[0].ToString(depth, Priority);
            for (int i = 1; i < Args.Length; i++)
            {
                s += separator + Args[i].ToString(depth, Priority);
            }
            return s;
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            var args = new Expression[Args.Length];
            for (var i = 0; i < Args.Length; i++)
            {
                args[i] = Args[i].EvaluateVars(excludeVariables);
            }
            return newInstanceWithOtherArgs(new object[] { args });
        }

        protected override Expression substituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions)
        {
            var args = new Expression[Args.Length];
            for (var i = 0; i < Args.Length; i++)
            {
                args[i] = Args[i].SubstituteVariables(cache, substitutions);
            }
            return newInstanceWithOtherArgs(new object[] { args });
        }
    }
}
