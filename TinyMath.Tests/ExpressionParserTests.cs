using FluentAssertions;
using Xunit;
using System.Linq;

namespace TinyMath.Tests
{
    public class ExpressionParserTests
    {
        [Theory]
        [InlineData("(4 + 5",5)]
        [InlineData("4 + 5(",5)]
        [InlineData("4) + 5",1)]
        [InlineData("4 + 5)",5)]
        [InlineData("4 $ 5",2)]
        [InlineData("4 +",2)]
        [InlineData("4 + (7 * (12 + 3)",16)]
        [InlineData("4 7",2)]
        [InlineData("funcCall(1, 2 * 3",16)]
        public void Parsing_Invalid_Expression_Indicates_Syntax_Error_Position(string expression, int position)
        {
            var action = () => ExpressionParser.Parse(expression);

            action.Should().Throw<ParserException>().Where(x => x.Position == position);
        }

        [Theory]
        [InlineData("-2",-2,"Simple negation")]
        [InlineData("5 + 3", 8, "Simple addition")]
        [InlineData("5 - 3", 2, "Simple subtraction")]
        [InlineData("5 - 1 - 2", 2, "Double subtraction")]
        [InlineData("2 * 3", 6, "Simple multimplication")]
        [InlineData("5 / 2", 2.5, "Simple division")]
        [InlineData("2 * 4+5", 13, "Multiply has higher order of precendence")]
        [InlineData("2 * (4+5)", 18, "Parentheses control order of precedence")]
        [InlineData("2 ^ 3", 8, "Simple pow")]
        [InlineData("2 ^ 3 * 4", 4096, "Simple pow with multiply")]
        [InlineData("(2 ^ 3) * 4", 32, "Simple pow with multiply")]
        [InlineData("2 ^ 3 ^ 2", 512, "Pow should be right associative")]        
        [InlineData("2 ^ -2",0.25,"Raising to negative exponent")]
        public void Evaluating_Numeric_Expression_Gives_Expected_Result(string expression, decimal expectedResult, string because)
        {
            var expr = ExpressionParser.Parse(expression);
            expr.Evaluate().Should().Be(expectedResult, because);
        }

        [Theory]
        [InlineData("foo", 2, "Just a variable")]
        [InlineData("-foo", -2, "Negate a variable")]
        [InlineData("foo * 2", 4, "Multiply a variable")]
        [InlineData("foo * foo", 4, "Square a variable")]
        [InlineData("multiply3(foo*2,foo,10)", 80, "Method call")]
        public void Evaluating_Variable_Expression_Gives_Expected_Result(string expression, decimal expectedResult, string because)
        {
            var expr = ExpressionParser.Parse(expression);
            expr.Evaluate(new Context()).Should().Be(expectedResult, because);
        }


        private class Context
        {
            public decimal Foo { get; } = 2.0m;

            public decimal Multiply3(decimal a, decimal b, decimal c) => a * b * c;
        }

        [Theory]
        [InlineData("foo", 2)]
        [InlineData("multiply3(foo*2,foo,10)", 80)]
        public void Evaluating_Expressions_Using_Resolver_Gves_Expected_Result(string expression, decimal expectedResult)
        {
            var expr = ExpressionParser.Parse(expression);
            var ctx = new ResolverExpressionContext();
            ctx.Variable = k =>
            {
                if (k == "foo")
                    return 2m;
                return 0m;
            };
            ctx.Function = k =>
            {
                if (k == "multiply3")
                    return args => args[0] * args[1] * args[2];
                return args => 0m;
            };

            expr.Evaluate(ctx).Should().Be(expectedResult);

        }

    }
}