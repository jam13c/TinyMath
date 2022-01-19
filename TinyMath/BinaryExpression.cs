using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    internal class BinaryExpression : IExpression
    {
        private readonly IExpression left;
        private readonly IExpression right;
        private readonly char op;

        public BinaryExpression(IExpression left, IExpression right, char op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }

        public decimal Evaluate(IExpressionContext ctx) => op switch
        {
            '*' => left.Evaluate(ctx) * right.Evaluate(ctx),
            '/' => left.Evaluate(ctx) / right.Evaluate(ctx),
            '+' => left.Evaluate(ctx) + right.Evaluate(ctx),
            '-' => left.Evaluate(ctx) - right.Evaluate(ctx),
            '^' => (decimal)Math.Pow((double)left.Evaluate(ctx), (double)right.Evaluate(ctx)),
            _ => throw new EvaluationException($"Unknown operator '{op}'")
        };
    }
}
