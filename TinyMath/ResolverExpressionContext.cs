using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public class ResolverExpressionContext : IExpressionContext
    {
        public Func<string, decimal> Variable { get; set; } = new Func<string, decimal>(key => throw new InvalidOperationException($"Unable to resolve variable '{key}'"));
        public Func<string, Func<decimal[], decimal>> Function { get; set; } = new Func<string, Func<decimal[], decimal>>(key => throw new InvalidOperationException($"Unable to resolve function '{key}'"));

        public decimal ExecuteFunction(string key, decimal[] arguments)
        {
            if (Function == null)
                throw new InvalidOperationException($"No Function resolver is registered");

            var func = Function(key);
            return func(arguments);
        }

        public decimal ResolveVariable(string key)
        {
            if (Variable == null)
                throw new InvalidOperationException($"No Variable resolver is registered");

            return Variable(key);
        }
    }
}
