using Mathematics.Math3D;

namespace Mathematics.Expressions
{
    public struct Point2DExpression
    {
        public Expression X, Y;

        public Point2DExpression(Expression x, Expression y)
        {
            X = x;
            Y = y;
        }

        public Point2DExpression(Point2D point)
        {
            X = point.X;
            Y = point.Y;
        }

        public Point2D Evaluate(int cacheGeneration = 0)
        {
            return new Point2D(X.Evaluate(cacheGeneration), Y.Evaluate(cacheGeneration));
        }
    }
}
