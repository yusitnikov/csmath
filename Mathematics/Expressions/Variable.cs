using System;
using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public class Variable : Expression
    {
        protected override Expression[] arguments
        {
            get => new Expression[] { };
            set { }
        }

        public readonly string Name;

        private double value = 0;
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public Variable(string name)
        {
            Name = name;
        }

        public Variable(string name, double value)
        {
            Name = name;
            Value = value;
        }

        internal override Priority Priority
        {
            get { return Priority.Single; }
        }

        protected override double evaluate()
        {
            return value;
        }

        public override double Evaluate()
        {
            return value;
        }

        public override string ToString()
        {
            return Name;
        }

        protected override Expression derivate(Variable variable)
        {
            return variable == this ? Constant.One : Constant.Nil;
        }

        public override Expression Derivate(Variable variable)
        {
            return variable == this ? Constant.One : Constant.Nil;
        }

        public override Expression Simplify()
        {
            return this;
        }

        public override Expression EvaluateVars(params Variable[] excludeVariables)
        {
            foreach (var excludeVariable in excludeVariables)
            {
                if (excludeVariable == this)
                {
                    return this;
                }
            }
            return new Constant(Evaluate());
        }

        public override Expression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions)
        {
            foreach (var substitution in substitutions)
            {
                if (substitution.Key == this)
                {
                    return substitution.Value;
                }
            }
            return this;
        }
    }
}
