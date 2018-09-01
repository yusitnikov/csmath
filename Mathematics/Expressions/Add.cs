using System;
using System.Linq;
using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public class Add : ManyArgsExpression
    {
        public Add(params Expression[] args) : base(args)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Add; }
        }

        protected override double evaluationInitValue => 0;

        protected override double evaluationStep(double prevValue, double value)
        {
            return prevValue + value;
        }

        protected override string separator => " + ";

        protected override Expression derivate(Variable variable)
        {
            return new Add(Args.Select(arg => arg.Derivate(variable)).ToArray());
        }

        public override Expression Simplify()
        {
            double constPart = 0;
            var varParts = new List<Expression>();
            Action<Add> add = null;
            add = sum =>
            {
                foreach (var arg in sum.Args)
                {
                    if (arg is Constant c)
                    {
                        constPart += c.Value;
                    }
                    else if (arg is Add a)
                    {
                        add(a);
                    }
                    else
                    {
                        varParts.Add(arg);
                    }
                }
            };
            add(this);
            if (varParts.Count == 0)
            {
                return new Constant(constPart);
            }
            if (constPart != 0)
            {
                varParts.Add(new Constant(constPart));
            }
            if (varParts.Count == 1)
            {
                return varParts[0];
            }
            return new Add(varParts.ToArray());
        }
    }
}
