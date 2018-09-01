using System.Collections.Generic;
using System.Linq;

namespace Mathematics.Expressions
{
    public abstract class Expression
    {
        private static object lockObject = new object();

        private static int autoIncrementId = 0;

        private static int currentCacheGeneration = 0;
        private int cacheEvalGeneration = 0;
        private double cachedEval = 0;
        private int cacheDerivateGeneration = 0;
        private Expression cachedDerivate = null;
        private Square squareCache = null;
        private Invert invertCache = null;

        protected abstract Expression[] arguments { get; set; }

        internal abstract Priority Priority { get; }

        public int PlainComplexity => 1 + arguments.Select(arg => arg.PlainComplexity).Sum();

        public int UniqueComplexity => getUniqueComplexity(new HashSet<string>());

        private int getUniqueComplexity(HashSet<string> set)
        {
            return set.Add(ToString()) ? 1 + arguments.Select(arg => arg.getUniqueComplexity(set)).Sum() : 0;
        }

        public void ReplaceSameSubExpressions()
        {
            replaceSameSubExpressions(new Dictionary<string, Expression>());
        }

        private void replaceSameSubExpressions(Dictionary<string, Expression> map)
        {
            arguments = arguments.Select(arg =>
            {
                var key = arg.ToString();
                if (map.TryGetValue(key, out Expression cachedArg))
                {
                    return cachedArg;
                }
                else
                {
                    map[key] = arg;
                    arg.replaceSameSubExpressions(map);
                    return arg;
                }
            }).ToArray();
        }

        protected abstract double evaluate();

        public virtual double Evaluate()
        {
            lock (lockObject)
            {
                if (currentCacheGeneration == 0)
                {
                    currentCacheGeneration = ++autoIncrementId;
                    try
                    {
                        return evaluate();
                    }
                    finally
                    {
                        currentCacheGeneration = 0;
                    }
                }
                else
                {
                    if (cacheEvalGeneration != currentCacheGeneration)
                    {
                        cachedEval = evaluate();
                        cacheEvalGeneration = currentCacheGeneration;
                    }
                    return cachedEval;
                }
            }
        }

        public abstract override string ToString();

        internal string ToString(Priority callerPriority)
        {
            string s = ToString();
            if (callerPriority >= Priority)
            {
                s = "(" + s + ")";
            }
            return s;
        }

        public abstract Expression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions);

        public virtual Expression Derivate(Variable variable)
        {
            lock (lockObject)
            {
                if (currentCacheGeneration == 0)
                {
                    currentCacheGeneration = ++autoIncrementId;
                    try
                    {
                        return derivate(variable);
                    }
                    finally
                    {
                        currentCacheGeneration = 0;
                    }
                }
                else
                {
                    if (cacheDerivateGeneration != currentCacheGeneration)
                    {
                        cachedDerivate = derivate(variable);
                        cacheDerivateGeneration = currentCacheGeneration;
                    }
                    return cachedDerivate;
                }
            }
        }

        protected abstract Expression derivate(Variable variable);

        public Point3DExpression Derivate(Point3DVariable p)
        {
            return new Point3DExpression(
                Derivate(p.X),
                Derivate(p.Y),
                Derivate(p.Z)
            ).Simplify();
        }

        public abstract Expression Simplify();

        public Square Square()
        {
            return squareCache ?? (squareCache = new Square(this));
        }

        public static Invert operator -(Expression arg)
        {
            return arg.invertCache ?? (arg.invertCache = new Invert(arg));
        }

        public static Add operator +(Expression arg1, Expression arg2)
        {
            return new Add(arg1, arg2);
        }

        public static Subtract operator -(Expression arg1, Expression arg2)
        {
            return new Subtract(arg1, arg2);
        }

        public static Multiply operator *(Expression arg1, Expression arg2)
        {
            return new Multiply(arg1, arg2);
        }

        public static Divide operator /(Expression arg1, Expression arg2)
        {
            return new Divide(arg1, arg2);
        }

        public static implicit operator Expression(double value)
        {
            return new Constant(value);
        }

        public abstract Expression EvaluateVars(params Variable[] excludeVariables);
    }
}
