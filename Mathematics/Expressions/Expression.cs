using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mathematics.Expressions
{
    public abstract class Expression
    {
        private static object lockObject = new object();

        private static int autoIncrementId = 0;
        public static int NextAutoIncrementId
        {
            get
            {
                int result;
                lock (lockObject)
                {
                    result = ++autoIncrementId;
                }
                return result;
            }
        }

        private int cacheEvalGeneration = 0;
        private double cachedEval = 0;
        private Dictionary<int, Expression> derivativeCache = new Dictionary<int, Expression>();

        public readonly int Id = NextAutoIncrementId;

        public string Alias { get; set; }

        protected abstract Expression[] arguments { get; set; }

        internal abstract Priority Priority { get; }

        public int PlainComplexity
        {
            get
            {
                var result = 1;
                foreach (var arg in arguments)
                {
                    result += arg.PlainComplexity;
                }
                return result;
            }
        }

        public int UniqueComplexity => getUniqueComplexity(new HashSet<string>());
        private int getUniqueComplexity(HashSet<string> set)
        {
            if (set.Add(ToString(int.MaxValue)))
            {
                var result = 1;
                foreach (var arg in arguments)
                {
                    result += arg.getUniqueComplexity(set);
                }
                return result;
            }
            else
            {
                return 0;
            }
        }

        protected virtual Expression newInstanceWithOtherArgs(params object[] args)
        {
            var ctor = GetType().GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
            return (ctor.Invoke(args) as Expression).Simplify();
        }

        protected abstract double evaluate(int cacheGeneration);

        public virtual double Evaluate(int cacheGeneration = 0)
        {
            if (cacheGeneration == 0)
            {
                cacheGeneration = NextAutoIncrementId;
            }

            lock (lockObject)
            {
                if (cacheEvalGeneration != cacheGeneration)
                {
                    cachedEval = evaluate(cacheGeneration);
                    cacheEvalGeneration = cacheGeneration;
                }
                return cachedEval;
            }
        }

        protected abstract string toString(int depth);
        public override string ToString()
        {
            return ToString(0);
        }
        internal string ToString(int depth)
        {
            if (depth == 0 && Alias != null)
            {
                return Alias;
            }
            else
            {
                if (Alias != null)
                {
                    --depth;
                }
                return toString(depth);
            }
        }
        internal string ToString(int depth, Priority callerPriority)
        {
            string s = ToString(depth);
            if ((depth != 0 || Alias == null) && callerPriority >= Priority)
            {
                s = "(" + s + ")";
            }
            return s;
        }

        protected abstract Expression substituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions);
        public Expression SubstituteVariables(Dictionary<int, Expression> cache, params KeyValuePair<Variable, Expression>[] substitutions)
        {
            var cacheKey = Id;
            if (cache.TryGetValue(cacheKey, out Expression value))
            {
                return value;
            }
            else
            {
                cache[cacheKey] = value = substituteVariables(cache, substitutions);
                value.Alias = value.Alias ?? Alias;
                return value;
            }
        }
        public Expression SubstituteVariables(params KeyValuePair<Variable, Expression>[] substitutions)
        {
            return SubstituteVariables(new Dictionary<int, Expression>(), substitutions);
        }

        public virtual Expression Derivate(Variable variable)
        {
            var cacheKey = variable.Id;

            if (derivativeCache.TryGetValue(cacheKey, out Expression value))
            {
                return value;
            }
            else
            {
                derivativeCache[cacheKey] = value = derivate(variable);
                if (value.Alias == null && Alias != null)
                {
                    value.Alias = Alias + "'";
                }
                return value;
            }
        }

        protected abstract Expression derivate(Variable variable);

        public Point3DExpression Derivate(Point3DVariable p)
        {
            return new Point3DExpression(
                Derivate(p.X),
                Derivate(p.Y),
                Derivate(p.Z)
            );
        }

        internal virtual Expression Simplify()
        {
            var isConstantExpression = !(this is Variable);
            foreach (var arg in arguments)
            {
                if (!(arg is Constant))
                {
                    isConstantExpression = false;
                    break;
                }
            }
            if (isConstantExpression)
            {
                return evaluate(-1);
            }
            else
            {
                return this;
            }
        }

        #region Functions

        public static Expression Acos(Expression x)
        {
            return new Acos(x).Simplify();
        }

        public static Expression Atan(Expression x)
        {
            return new Atan(x).Simplify();
        }

        public static Expression Atan2(Expression y, Expression x)
        {
            return new Atan2(y, x).Simplify();
        }

        public static Expression BinarySearch(Expression expression, Variable variable, Expression min, Expression max, Expression precision, Func<double, int, bool> comparer = null)
        {
            return new BinarySearchExpression(expression, variable, min, max, precision, comparer);
        }

        public static Expression Ci(Expression x)
        {
            return new Ci(x).Simplify();
        }

        public static Expression Coalesce(Expression conditionValue, Expression valueIfNil, Expression valueIfNotNil)
        {
            return new Coalesce(conditionValue, valueIfNil, valueIfNotNil).Simplify();
        }

        public static Expression Cos(Expression x)
        {
            return new Cos(x).Simplify();
        }

        public static Expression Exp(Expression x)
        {
            return new Exp(x).Simplify();
        }

        public static Expression Hypot(params Expression[] args)
        {
            return new Hypot(args).Simplify();
        }

        public static Expression IfPositive(Expression conditionValue, Expression valueIfPositive, Expression valueIfNegative)
        {
            return new IfPositive(conditionValue, valueIfPositive, valueIfNegative).Simplify();
        }

        public static Expression Log(Expression x)
        {
            return new Log(x).Simplify();
        }

        public static Expression Max(Expression x, Expression y)
        {
            return new Max(x, y).Simplify();
        }

        public static Expression Min(Expression x, Expression y)
        {
            return new Min(x, y).Simplify();
        }

        public static Expression Si(Expression x)
        {
            return new Si(x).Simplify();
        }

        public static Expression Sin(Expression x)
        {
            return new Sin(x).Simplify();
        }

        public static Expression Sqrt(Expression x)
        {
            return new Sqrt(x).Simplify();
        }

        public static Expression Sum(params Expression[] args)
        {
            return new Add(args).Simplify();
        }

        public static Expression Product(params Expression[] args)
        {
            return new Multiply(args).Simplify();
        }

        public Expression Square()
        {
            return new Square(this).Simplify();
        }

        #endregion

        #region Operators

        public static Expression operator -(Expression arg)
        {
            return new Invert(arg).Simplify();
        }

        public static Expression operator +(Expression arg1, Expression arg2)
        {
            return new Add(arg1, arg2).Simplify();
        }

        public static Expression operator -(Expression arg1, Expression arg2)
        {
            return new Subtract(arg1, arg2).Simplify();
        }

        public static Expression operator *(Expression arg1, Expression arg2)
        {
            return new Multiply(arg1, arg2).Simplify();
        }

        public static Expression operator /(Expression arg1, Expression arg2)
        {
            return new Divide(arg1, arg2).Simplify();
        }

        public static implicit operator Expression(double value)
        {
            return new Constant(value);
        }

        #endregion

        public abstract Expression EvaluateVars(params Variable[] excludeVariables);
    }
}
