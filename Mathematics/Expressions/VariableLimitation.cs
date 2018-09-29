namespace Mathematics.Expressions
{
    public class VariableLimitation
    {
        public Variable Variable;
        public double Limit;
        public int Sign;

        public virtual bool IsExceeded(double diff = 0) => (Variable.Value + diff - Limit) * Sign > 0;
        public virtual double GetDiff() => Limit - Variable.Value;
    }
}
