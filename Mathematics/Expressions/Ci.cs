namespace Mathematics.Expressions
{
    public class Ci : OneArgFunction
    {
        public Ci(Expression arg) : base("Ci", arg)
        {
        }

        protected override double _evaluate(double value)
        {
            return Complex.ExpIntOfImaginaryArg(value).Re;
        }

        protected override Expression _derivate()
        {
            return new Cos(Arg) / Arg;
        }
    }
}
