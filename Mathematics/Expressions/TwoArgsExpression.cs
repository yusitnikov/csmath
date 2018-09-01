using System;
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
            Arg1 = arg1.Simplify();
            Arg2 = arg2.Simplify();
        }

        protected abstract double _evaluate(double arg1, double arg2);

        protected override double evaluate()
        {
            return _evaluate(Arg1.Evaluate(), Arg2.Evaluate());
        }

        protected virtual Expression newInstanceWithOtherArgs(Expression arg1, Expression arg2)
        {
            return (GetType().GetConstructor(new Type[] { typeof(Expression), typeof(Expression) }).Invoke(new object[] { arg1, arg2 }) as Expression);
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return newInstanceWithOtherArgs(Arg1.EvaluateVars(excludeVariables).Simplify(), Arg2.EvaluateVars(excludeVariables).Simplify()).Simplify();
        }

        public override Expression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return newInstanceWithOtherArgs(Arg1.SubstituteVariables(substitutions), Arg2.SubstituteVariables(substitutions));
        }
    }
}
