using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public class Coalesce : Expression
    {
        protected override Expression[] arguments
        {
            get => new Expression[] { ConditionValue, ValueIfNil, ValueIfNotNil };
            set
            {
                ConditionValue = value[0];
                ValueIfNil = value[1];
                ValueIfNotNil = value[2];
            }
        }

        public Expression ConditionValue { get; private set; }
        public Expression ValueIfNil { get; private set; }
        public Expression ValueIfNotNil { get; private set; }

        internal override Priority Priority
        {
            get { return Priority.Function; }
        }

        internal Coalesce(Expression conditionValue, Expression valueIfNil, Expression valueIfNotNil)
        {
            ConditionValue = conditionValue;
            ValueIfNil = valueIfNil;
            ValueIfNotNil = valueIfNotNil;
        }

        protected override double evaluate(int cacheGeneration)
        {
            return ConditionValue.Evaluate(cacheGeneration) == 0 ? ValueIfNil.Evaluate(cacheGeneration) : ValueIfNotNil.Evaluate(cacheGeneration);
        }

        protected override string toString(int depth)
        {
            return "if(0 = " + ConditionValue.ToString(depth) + "; " + ValueIfNil.ToString(depth) + "; " + ValueIfNotNil.ToString(depth) + ")";
        }

        protected override Expression derivate(Variable variable)
        {
            return Coalesce(ConditionValue, ValueIfNil.Derivate(variable), ValueIfNotNil.Derivate(variable));
        }

        internal override Expression Simplify()
        {
            if (ConditionValue is Constant c)
            {
                return c.Value == 0 ? ValueIfNil : ValueIfNotNil;
            }
            return base.Simplify();
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return Coalesce(ConditionValue.EvaluateVars(excludeVariables), ValueIfNil.EvaluateVars(excludeVariables), ValueIfNotNil.EvaluateVars(excludeVariables));
        }

        protected override Expression substituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return Coalesce(ConditionValue.SubstituteVariables(cache, substitutions), ValueIfNil.SubstituteVariables(cache, substitutions), ValueIfNotNil.SubstituteVariables(cache, substitutions));
        }
    }
}
