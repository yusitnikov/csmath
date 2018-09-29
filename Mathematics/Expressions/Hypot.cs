using System;

namespace Mathematics.Expressions
{
    public class Hypot : ManyArgsExpression
    {
        internal Hypot(params Expression[] args) : base(args)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Function; }
        }

        protected override double evaluate(int cacheGeneration)
        {
            double result = 0;
            foreach (var arg in Args)
            {
                var value = arg.Evaluate(cacheGeneration);
                result += value * value;
            }
            return Math.Sqrt(result);
        }

        protected override string separator => ", ";

        protected override string toString(int depth)
        {
            return "hypot(" + base.toString(depth) + ")";
        }

        protected override Expression derivate(Variable variable)
        {
            // (sqrt(x^2 + y^2 + ...))' = (x*x' + y*y' + ...) / sqrt(x^2 + y^2 + ...)
            var derivatives = new Expression[Args.Length];
            for (var i = 0; i < Args.Length; i++)
            {
                var arg = Args[i];
                derivatives[i] = arg * arg.Derivate(variable);
            }
            return Sum(derivatives) / this;
        }
    }
}
