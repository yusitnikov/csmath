namespace Mathematics.Expressions
{
    public class Multiply : ManyArgsExpression
    {
        internal Multiply(params Expression[] args) : base(args)
        {
        }

        internal override Priority Priority
        {
            get { return Priority.Multiply; }
        }

        protected override double evaluate(int cacheGeneration)
        {
            double result = 1;
            foreach (var arg in Args)
            {
                result *= arg.Evaluate(cacheGeneration);
                if (result == 0)
                {
                    break;
                }
            }
            return result;
        }

        protected override string separator { get { return " * "; } }

        protected override Expression derivate(Variable variable)
        {
            if (Args.Length == 2)
            {
                var arg1 = Args[0];
                var arg2 = Args[1];
                return arg2 * arg1.Derivate(variable) + arg1 * arg2.Derivate(variable);
            }

            Expression[] parts = new Expression[Args.Length];
            for (int i = 0; i < Args.Length; i++)
            {
                var arg = Args[i];
                Args[i] = arg.Derivate(variable);
                parts[i] = Product(Args);
                Args[i] = arg;
            }
            return Sum(parts);
        }

        internal override Expression Simplify()
        {
            foreach (var arg in Args)
            {
                if (arg is Constant c && c.Value == 0)
                {
                    return Constant.Nil;
                }
            }
            return base.Simplify();
        }
    }
}
