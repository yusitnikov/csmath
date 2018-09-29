using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public class IfPositive : Expression
    {
        protected override Expression[] arguments
        {
            get => new Expression[] { ConditionValue, ValueIfPositive, ValueIfNegative };
            set
            {
                ConditionValue = value[0];
                ValueIfPositive = value[1];
                ValueIfNegative = value[2];
            }
        }

        public Expression ConditionValue { get; private set; }
        public Expression ValueIfPositive { get; private set; }
        public Expression ValueIfNegative { get; private set; }

        internal override Priority Priority
        {
            get { return Priority.Function; }
        }

        internal IfPositive(Expression conditionValue, Expression valueIfPositive, Expression valueIfNegative)
        {
            ConditionValue = conditionValue;
            ValueIfPositive = valueIfPositive;
            ValueIfNegative = valueIfNegative;
        }

        protected override double evaluate(int cacheGeneration)
        {
            return ConditionValue.Evaluate(cacheGeneration) > 0 ? ValueIfPositive.Evaluate(cacheGeneration) : ValueIfNegative.Evaluate(cacheGeneration);
        }

        protected override string toString(int depth)
        {
            return "if(0 < " + ConditionValue.ToString(depth) + "; " + ValueIfPositive.ToString(depth) + "; " + ValueIfNegative.ToString(depth) + ")";
        }

        protected override Expression derivate(Variable variable)
        {
            return IfPositive(ConditionValue, ValueIfPositive.Derivate(variable), ValueIfNegative.Derivate(variable));
        }

        internal override Expression Simplify()
        {
            if (ConditionValue is Constant c)
            {
                return c.Value > 0 ? ValueIfPositive : ValueIfNegative;
            }
            return base.Simplify();
        }

        protected override Expression substituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return IfPositive(
                ConditionValue.SubstituteVariables(cache, substitutions),
                ValueIfPositive.SubstituteVariables(cache, substitutions),
                ValueIfNegative.SubstituteVariables(cache, substitutions)
            );
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return IfPositive(
                ConditionValue.EvaluateVars(excludeVariables),
                ValueIfPositive.EvaluateVars(excludeVariables),
                ValueIfNegative.EvaluateVars(excludeVariables)
            );
        }
    }
}
