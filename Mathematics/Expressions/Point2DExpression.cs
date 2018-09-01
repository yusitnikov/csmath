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
            X = new Constant(point.X);
            Y = new Constant(point.Y);
        }

        public Point2D Evaluate()
        {
            return new Point2D(X.Evaluate(), Y.Evaluate());
        }
    }
}
