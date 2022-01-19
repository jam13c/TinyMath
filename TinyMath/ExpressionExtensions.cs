using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public static class ExpressionExtensions
    {
        public static decimal Evaluate(this IExpression expr) => expr.Evaluate(new NullExpressionContext());

        public static decimal Evaluate(this IExpression expr, Dictionary<string, decimal> context) => expr.Evaluate(new DictionaryExpressionContext(context));

        public static decimal Evaluate(this IExpression expr, object context) => expr.Evaluate(new ReflectionExpressionContext(context));

        private class NullExpressionContext : IExpressionContext
        {
            public decimal ExecuteFunction(string key, decimal[] arguments)
            {
                throw new NotImplementedException();
            }

            public decimal ResolveVariable(string key)
            {
                throw new NotImplementedException();
            }
        }
    }
}
