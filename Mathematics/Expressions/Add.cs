namespace Mathematics.Expressions
{
    public class Add : ManyArgsExpression
    {
        internal Add(params Expression[] args) : base(args)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Add; }
        }

        protected override double evaluate(int cacheGeneration)
        {
            double result = 0;
            foreach (var arg in Args)
            {
                result += arg.Evaluate(cacheGeneration);
            }
            return result;
        }

        protected override string separator => " + ";

        protected override Expression derivate(Variable variable)
        {
            var derivatives = new Expression[Args.Length];
            for (var i = 0; i < Args.Length; i++)
            {
                derivatives[i] = Args[i].Derivate(variable);
            }
            return Sum(derivatives);
        }
    }
}
