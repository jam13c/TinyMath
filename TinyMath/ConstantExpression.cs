using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    internal class ConstantExpression : IExpression
    {
        private readonly decimal value;

        public ConstantExpression(decimal value)
        {
            this.value = value;
        }

        public decimal Evaluate(IExpressionContext ctx) => value;
    }
}
