using System;
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

        public Constant(double value)
        {
            Value = value;
        }

        internal override Priority Priority
        {
            get { return Value >= 0 ? Priority.Single : Priority.Add; }
        }

        protected override double evaluate()
        {
            return Value;
        }

        public override double Evaluate()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override Expression derivate(Variable variable)
        {
            return Constant.Nil;
        }

        public override Expression Derivate(Variable variable)
        {
            return Constant.Nil;
        }

        public override Expression Simplify()
        {
            return this;
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            return this;
        }

        public override Expression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return this;
        }
    }
}
