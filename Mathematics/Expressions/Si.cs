namespace Mathematics.Expressions
{
    public class Si : OneArgFunction
    {
        internal Si(Expression arg) : base("si", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Complex.ExpIntOfImaginaryArg(value).Im;
        }

        protected override Expression _derivate()
        {
            return Sin(Arg) / Arg;
        }
    }
}
