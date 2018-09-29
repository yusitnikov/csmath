using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public abstract class OneArgExpression : Expression
    {
        protected override Expression[] arguments
        {
            get => new Expression[] { Arg };
            set => Arg = value[0];
        }

        public Expression Arg { get; private set; }

        public OneArgExpression(Expression arg)
        {
            Arg = arg;
        }

        protected abstract double _evaluate(double val);

        protected override double evaluate(int cacheGeneration)
        {
            return _evaluate(Arg.Evaluate(cacheGeneration));
        }

        protected abstract Expression _derivate();

        protected override Expression derivate(Variable variable)
        {
            return _derivate() * Arg.Derivate(variable);
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return newInstanceWithOtherArgs(Arg.EvaluateVars(excludeVariables));
        }

        protected override Expression substituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return newInstanceWithOtherArgs(Arg.SubstituteVariables(cache, substitutions));
        }
    }
}
