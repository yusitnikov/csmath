using System;
using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public class BinarySearchExpression : Expression
    {
        protected override Expression[] arguments
        {
            get => new Expression[] { Expression };
            set => Expression = value[0];
        }

        public Expression Expression { get; private set; }
        public Variable Variable { get; private set; }
        public new Expression Min { get; private set; }
        public new Expression Max { get; private set; }
        public Expression Precision { get; private set; }
        public Func<double, int, bool> Comparer { get; private set; }

        internal override Priority Priority
        {
            get { return Priority.Function; }
        }

        internal BinarySearchExpression(Expression expression, Variable variable, Expression min, Expression max, Expression precision, Func<double, int, bool> comparer = null)
        {
            Expression = expression;
            Variable = variable;
            Min = min;
            Max = max;
            Precision = precision;
            Comparer = comparer;
        }

        protected override double evaluate(int cacheGeneration)
        {
            return Mathematics.BinarySearch.Search(
                Min.Evaluate(cacheGeneration), Max.Evaluate(cacheGeneration), Precision.Evaluate(cacheGeneration),
                x =>
                {
                    Variable.Value = x;
                    var newCacheGeneration = NextAutoIncrementId;
                    return (Comparer == null || Comparer(x, newCacheGeneration)) && Expression.Evaluate(newCacheGeneration) < 0;
                },
                x =>
                {
                    Variable.Value = x;
                    return Expression.Evaluate();
                }
            );
        }

        protected override string toString(int depth)
        {
            return "BinSearch(" + Variable.ToString(depth) + "; " + Expression.ToString(depth) + ")";
        }

        protected override Expression derivate(Variable x)
        {
            if (x == Variable)
            {
                return Constant.Nil;
            }

            // f(x, t) = 0 => df(x, t) = 0
            // f'_x(x, t) dx + f'_t(x, t) dt = 0
            // t'_x = -f'_x(x, t) / f'_t(x, t)
            Expression df = -Expression.Derivate(x) / Expression.Derivate(Variable);
            return df.SubstituteVariables(new KeyValuePair<Variable, Expression>(Variable, this));
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            // TODO: check
            return new BinarySearchExpression(Expression.EvaluateVars(excludeVariables), Variable, Min, Max, Precision, Comparer);
        }

        protected override Expression substituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions)
        {
            // TODO: check
            return new BinarySearchExpression(Expression.SubstituteVariables(cache, substitutions), Variable, Min, Max, Precision, Comparer);
        }
    }
}
