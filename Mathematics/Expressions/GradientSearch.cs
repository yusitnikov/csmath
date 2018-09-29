using System;
using System.Collections.Generic;

namespace Mathematics.Expressions
{
    public static class GradientSearch
    {
        public struct EvalResult
        {
            public double Value;
            public double[] Derivatives;
        }

        public static bool Step(Func<EvalResult> criteriaEvaluator, IList<Variable> variables, IList<VariableLimitation> limitations, double absoluteStep, double relativeStep)
        {
            var variablesCount = variables.Count;
            var activeVariablesCount = variablesCount;

            var variablesUsage = new bool[variablesCount];
            var variablesIndexMap = new Dictionary<int, int>();
            for (var variableIndex = 0; variableIndex < variablesCount; variableIndex++)
            {
                variablesUsage[variableIndex] = true;
                variablesIndexMap.Add(variables[variableIndex].Id, variableIndex);
            }

            start:

            var criteria = criteriaEvaluator();

            var stepVector = new double[variablesCount];

            for (var variableIndex = 0; variableIndex < variablesCount; variableIndex++)
            {
                if (variablesUsage[variableIndex])
                {
                    stepVector[variableIndex] -= criteria.Derivatives[variableIndex];
                }
            }

            double scale = 0;
            for (var variableIndex = 0; variableIndex < variablesCount; variableIndex++)
            {
                if (variablesUsage[variableIndex])
                {
                    scale += criteria.Derivatives[variableIndex] * stepVector[variableIndex];
                }
            }

            if (scale == 0)
            {
                // It shouldn't happen
                return false;
            }
            var step = Math.Max(absoluteStep, relativeStep * Math.Abs(criteria.Value));
            var stepCoeff = step / Math.Abs(scale);

            VariableLimitation failedLimitation = null;
            foreach (var limitation in limitations)
            {
                var variableIndex = variablesIndexMap[limitation.Variable.Id];
                if (variablesUsage[variableIndex])
                {
                    var variableStep = stepVector[variableIndex];
                    if (limitation.IsExceeded(variableStep * stepCoeff))
                    {
                        stepCoeff = limitation.GetDiff() / variableStep;
                        failedLimitation = limitation;
                    }
                }
            }

            if (failedLimitation != null)
            {
                failedLimitation.Variable.Value = failedLimitation.Limit;
                variablesUsage[variablesIndexMap[failedLimitation.Variable.Id]] = false;
                --activeVariablesCount;
                if (activeVariablesCount == 0)
                {
                    return true;
                }
                goto start;
            }

            for (var variableIndex = 0; variableIndex < variablesCount; variableIndex++)
            {
                if (variablesUsage[variableIndex])
                {
                    variables[variableIndex].Value += stepVector[variableIndex] * stepCoeff;
                }
            }

            return true;
        }

        public static bool Step(Expression criteria, IList<Variable> variables, IList<VariableLimitation> limitations, double absoluteStep, double relativeStep, int cacheGeneration = 0)
        {
            if (cacheGeneration == 0)
            {
                cacheGeneration = Expression.NextAutoIncrementId;
            }

            var variablesCount = variables.Count;
            var derivatives = new Expression[variablesCount];
            for (var variableIndex = 0; variableIndex < variablesCount; variableIndex++)
            {
                derivatives[variableIndex] = criteria.Derivate(variables[variableIndex]);
            }
            return Step(
                () =>
                {
                    var result = new EvalResult()
                    {
                        Value = criteria.Evaluate(cacheGeneration),
                        Derivatives = new double[variablesCount]
                    };
                    for (var variableIndex = 0; variableIndex < variablesCount; variableIndex++)
                    {
                        result.Derivatives[variableIndex] = derivatives[variableIndex].Evaluate(cacheGeneration);
                    }
                    cacheGeneration = Expression.NextAutoIncrementId;
                    return result;
                },
                variables, limitations, absoluteStep, relativeStep
            );
        }
    }
}
