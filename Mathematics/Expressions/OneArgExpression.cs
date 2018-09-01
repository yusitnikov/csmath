using System;
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
            Arg = arg.Simplify();
        }

        protected abstract double _evaluate(double val);

        protected override double evaluate()
        {
            return _evaluate(Arg.Evaluate());
        }

        protected abstract Expression _derivate();

        protected override Expression derivate(Variable variable)
        {
            return _derivate() * Arg.Derivate(variable);
        }

        public override Expression Simplify()
        {
            if (Arg is Constant)
            {
                return new Constant(Evaluate());
            }
            return this;
        }

        protected virtual Expression newInstanceWithOtherArg(Expression arg)
        {
            return GetType().GetConstructor(new Type[] { typeof(Expression) }).Invoke(new object[] { arg }) as Expression;
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return newInstanceWithOtherArg(Arg.EvaluateVars(excludeVariables).Simplify()).Simplify();
        }

        public override Expression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return newInstanceWithOtherArg(Arg.SubstituteVariables(substitutions));
        }
    }
}
