using System;
using System.Linq;
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
            Args = args.Select(arg => arg.Simplify()).ToArray();
        }

        protected abstract double evaluationInitValue { get; }

        protected abstract double evaluationStep(double prevValue, double value);

        protected override double evaluate()
        {
            double r = evaluationInitValue;
            foreach (Expression arg in Args)
            {
                r = evaluationStep(r, arg.Evaluate());
            }
            return r;
        }

        protected abstract string separator { get; }

        public override string ToString()
        {
            string s = Args[0].ToString(Priority);
            for (int i = 1; i < Args.Length; i++)
            {
                s += separator + Args[i].ToString(Priority);
            }
            return s;
        }

        protected virtual Expression newInstanceWithModifiedArgs(Func<Expression, Expression> converter)
        {
            var args = Args.Select(converter).ToArray();
            return GetType().GetConstructor(new Type[] { typeof(Expression[]) }).Invoke(new object[] { args }) as Expression;
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return newInstanceWithModifiedArgs(arg => arg.EvaluateVars(excludeVariables).Simplify()).Simplify();
        }

        public override Expression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return newInstanceWithModifiedArgs(arg => arg.SubstituteVariables(substitutions));
        }
    }
}
