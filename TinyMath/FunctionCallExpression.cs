using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public class FunctionCallExpression : IExpression
    {
        private string name;
        private readonly IEnumerable<IExpression> arguments;

        public FunctionCallExpression(string name, IEnumerable<IExpression> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public decimal Evaluate(IExpressionContext ctx) => ctx.ExecuteFunction(name.ToString(), arguments.Select(a => a.Evaluate(ctx) ).ToArray());
    }
}
