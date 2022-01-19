using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public class UnaryExpression : IExpression
    {
        private readonly IExpression expr;
        private readonly char op;

        public UnaryExpression(IExpression expr, char op)
        {
            this.expr = expr;
            this.op = op;
        }

        public decimal Evaluate(IExpressionContext ctx) => op switch
        {
            '-' => -expr.Evaluate(ctx),
            _ => throw new EvaluationException($"Unexpected unary operator '{op}'")
        };
    }
}
