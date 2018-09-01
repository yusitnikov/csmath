using System;
using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public class Multiply : ManyArgsExpression
    {
        public Multiply(params Expression[] args) : base(args)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Multiply; }
        }

        protected override double evaluationInitValue { get { return 1; } }

        protected override double evaluationStep(double prevValue, double value)
        {
            return prevValue * value;
        }

        protected override string separator { get { return " * "; } }

        protected override Expression derivate(Variable variable)
        {
            Expression[] parts = new Expression[Args.Length];
            for (int i = 0; i < Args.Length; i++)
            {
                Expression[] args = Args.Clone() as Expression[];
                args[i] = args[i].Derivate(variable);
                parts[i] = new Multiply(args);
            }
            return new Add(parts);
        }

        public override Expression Simplify()
        {
            double constPart = 1;
            var varParts = new List<Expression>();
            Action<Multiply> add = null;
            add = mult =>
            {
                foreach (var arg in mult.Args)
                {
                    if (arg is Constant)
                    {
                        constPart *= arg.Evaluate();
                    }
                    else if (arg is Multiply)
                    {
                        add(arg as Multiply);
                    }
                    else
                    {
                        varParts.Add(arg);
                    }
                }
            };
            add(this);
            if (constPart == 0)
            {
                return Constant.Nil;
            }
            if (varParts.Count == 0)
            {
                return new Constant(constPart);
            }
            if (constPart != 1)
            {
                varParts.Insert(0, new Constant(constPart));
            }
            if (varParts.Count == 1)
            {
                return varParts[0];
            }
            return new Multiply(varParts.ToArray());
        }
    }
}
