namespace Mathematics.Expressions
{
    public class Si : OneArgFunction
    {
        public Si(Expression arg) : base("si", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Complex.ExpIntOfImaginaryArg(value).Im;
        }

        protected override Expression _derivate()
        {
            return new Sin(Arg) / Arg;
        }
    }
}
