using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    internal class VariableExpression : IExpression
    {
        private string name;

        public VariableExpression(string name)
        {
            this.name = name;
        }

        public decimal Evaluate(IExpressionContext ctx) => ctx.ResolveVariable(name);
    }
}
