using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public abstract class TwoArgsExpression : Expression
    {
        protected override Expression[] arguments
        {
            get => new Expression[] { Arg1, Arg2 };
            set
            {
                Arg1 = value[0];
                Arg2 = value[1];
            }
        }

        public Expression Arg1 { get; private set; }
        public Expression Arg2 { get; private set; }

        public TwoArgsExpression(Expression arg1, Expression arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }

        protected abstract double _evaluate(double arg1, double arg2);

        protected override double evaluate(int cacheGeneration)
        {
            return _evaluate(Arg1.Evaluate(cacheGeneration), Arg2.Evaluate(cacheGeneration));
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return newInstanceWithOtherArgs(Arg1.EvaluateVars(excludeVariables), Arg2.EvaluateVars(excludeVariables));
        }

        protected override Expression substituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return newInstanceWithOtherArgs(Arg1.SubstituteVariables(cache, substitutions), Arg2.SubstituteVariables(cache, substitutions));
        }
    }
}
