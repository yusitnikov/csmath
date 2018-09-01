using System;
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

        public Coalesce(Expression conditionValue, Expression valueIfNil, Expression valueIfNotNil)
        {
            ConditionValue = conditionValue;
            ValueIfNil = valueIfNil;
            ValueIfNotNil = valueIfNotNil;
        }

        protected override double evaluate()
        {
            return ConditionValue.Evaluate() == 0 ? ValueIfNil.Evaluate() : ValueIfNotNil.Evaluate();
        }

        public override string ToString()
        {
            return "coalesce(" + ConditionValue.ToString(Priority) + "; " + ValueIfNil.ToString(Priority) + "; " + ValueIfNotNil.ToString(Priority) + ")";
        }

        protected override Expression derivate(Variable variable)
        {
            return new Coalesce(ConditionValue, ValueIfNil.Derivate(variable), ValueIfNotNil.Derivate(variable));
        }

        public override Expression Simplify()
        {
            var simplifiedConditionValue = ConditionValue.Simplify();
            if (simplifiedConditionValue is Constant)
            {
                return simplifiedConditionValue.Evaluate() == 0 ? ValueIfNil.Simplify() : ValueIfNotNil.Simplify();
            }
            return new Coalesce(simplifiedConditionValue, ValueIfNil.Simplify(), ValueIfNotNil.Simplify());
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return new Coalesce(ConditionValue.EvaluateVars(excludeVariables), ValueIfNil.EvaluateVars(excludeVariables), ValueIfNotNil.EvaluateVars(excludeVariables)).Simplify();
        }

        public override Expression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return new Coalesce(ConditionValue.SubstituteVariables(substitutions), ValueIfNil.SubstituteVariables(substitutions), ValueIfNotNil.SubstituteVariables(substitutions)).Simplify();
        }
    }
}
