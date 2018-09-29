using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public class Constant : Expression
    {
        public static readonly Constant Nil = new Constant(0), One = new Constant(1), Two = new Constant(2), Half = new Constant(0.5), MinusOne = new Constant(-1), MinusHalf = new Constant(-0.5);

        protected override Expression[] arguments
        {
            get => new Expression[] { };
            set { }
        }

        public readonly double Value;

        internal Constant(double value)
        {
            Value = value;
        }

        internal override Priority Priority
        {
            get { return Value >= 0 ? Priority.Single : Priority.Add; }
        }

        protected override double evaluate(int cacheGeneration)
        {
            return Value;
        }

        public override double Evaluate(int cacheGeneration = 0)
        {
            return Value;
        }

        protected override string toString(int depth)
        {
            return Value.ToString();
        }

        protected override Expression derivate(Variable variable)
        {
            return Nil;
        }

        public override Expression Derivate(Variable variable)
        {
            return Nil;
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return this;
        }

        protected override Expression substituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return this;
        }

        internal override Expression Simplify()
        {
            return this;
        }
    }
}
