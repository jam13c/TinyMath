using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public static class ExpressionParser
    {
        public static IExpression Parse(ReadOnlySpan<char> expr)
        {
            var tokenizer = new Tokenizer(expr);
            var interpreter = new Interpreter(tokenizer);
            return interpreter.ParseExpression();
        }
    }
}
