using Mathematics.Math3D;

namespace Mathematics.Expressions
{
    public struct Point3DVariable
    {
        public static Point3DVariable Empty => new Point3DVariable(Point3D.Empty);

        public Variable X, Y, Z;

        public Point3DVariable(Variable x, Variable y, Variable z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3DVariable(Point3D point, string variableName = "", string xAxisName = "x", string yAxisName = "y", string zAxisName = "z")
        {
            X = new Variable(variableName + xAxisName, point.X);
            Y = new Variable(variableName + yAxisName, point.Y);
            Z = new Variable(variableName + zAxisName, point.Z);
        }

        public void Update(Point3D point)
        {
            X.Value = point.X;
            Y.Value = point.Y;
            Z.Value = point.Z;
        }

        public Point3D Evaluate(int cacheGeneration = 0)
        {
            return new Point3D(X.Evaluate(cacheGeneration), Y.Evaluate(cacheGeneration), Z.Evaluate(cacheGeneration));
        }

        public Point3DExpression EvaluateVars(params Variable[] excludeVariables)
        {
            return new Point3DExpression(
                X.EvaluateVars(excludeVariables),
                Y.EvaluateVars(excludeVariables),
                Z.EvaluateVars(excludeVariables)
            );
        }

        public static implicit operator Point3DExpression(Point3DVariable v)
        {
            return new Point3DExpression(v.X, v.Y, v.Z);
        }

        public static implicit operator Point3DVariable(Point3DExpression e)
        {
            return new Point3DVariable(e.X as Variable, e.Y as Variable, e.Z as Variable);
        }
    }
}
