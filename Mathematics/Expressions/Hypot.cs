using System;
using System.Linq;
using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public class Hypot : ManyArgsExpression
    {
        public Hypot(params Expression[] args) : base(args)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Function; }
        }

        protected override double evaluationInitValue => 0;

        protected override double evaluationStep(double prevValue, double value)
        {
            return prevValue + value * value;
        }

        protected override double evaluate()
        {
            return Math.Sqrt(base.evaluate());
        }

        protected override string separator => ", ";

        public override string ToString()
        {
            return "hypot(" + base.ToString() + ")";
        }

        protected override Expression derivate(Variable variable)
        {
            // (sqrt(x^2 + y^2 + ...))' = (x*x' + y*y' + ...) / sqrt(x^2 + y^2 + ...)
            return new Add(Args.Select(arg => arg * arg.Derivate(variable)).ToArray()) / this;
        }

        public override Expression Simplify()
        {
            double constPart = 0;
            var varParts = new List<Expression>();
            foreach (var arg in Args)
            {
                if (arg is Constant c)
                {
                    var val = c.Value;
                    constPart += val * val;
                }
                else
                {
                    varParts.Add(arg);
                }
            }
            constPart = Math.Sqrt(constPart);
            if (varParts.Count == 0)
            {
                return new Constant(constPart);
            }
            if (constPart != 0)
            {
                varParts.Add(new Constant(constPart));
            }
            return new Hypot(varParts.ToArray());
        }
    }
}
