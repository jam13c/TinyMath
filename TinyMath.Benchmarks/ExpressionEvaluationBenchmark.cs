using BenchmarkDotNet.Attributes;
using Flee.PublicTypes;
using Jace;
using org.mariuszgromada.math.mxparser;
using Soukoku.ExpressionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath.Benchmarks
{
    [MemoryDiagnoser]
    public class ExpressionEvaluationBenchmark
    {
        readonly static string ExpressionToUse = "foo * (bar / 10) + 42";

        [Benchmark(Baseline = true)]
        public decimal TinyMath()
        {
            var expr = ExpressionParser.Parse(ExpressionToUse);
            return expr.Evaluate(new DictionaryExpressionContext()
            {
                {"foo",1.0m},
                {"bar", 2.0m}
            });
        }

        [Benchmark]
        public double Jace()
        {
            Dictionary<string, double> variables = new Dictionary<string, double>();
            variables.Add("foo", 1.0);
            variables.Add("bar", 2.0);

            CalculationEngine engine = new CalculationEngine();
            return engine.Calculate(ExpressionToUse, variables);
        }

        [Benchmark]
        public decimal Flee()
        {
            var ctx = new ExpressionContext();
            ctx.Variables["foo"] = 1.0m;
            ctx.Variables["bar"] = 2.0m;
            var expr = ctx.CompileGeneric<decimal>(ExpressionToUse);
            return expr.Evaluate();
        }

        [Benchmark]
        public decimal Soukoku()
        {
            var context = new EvaluationContext();

            context.RegisterFunction("foo", new FunctionRoutine(0, (ctx, parameters) => new ExpressionToken("1.0")));
            context.RegisterFunction("bar", new FunctionRoutine(0, (ctx, parameters) => new ExpressionToken("2.0")));
            var evaluator = new Evaluator(context);
            var result = evaluator.Evaluate("foo() * bar() / 10 + 42");
            return result.ToDecimal(context);
        }

        [Benchmark]
        public double MX()
        {
            Argument foo = new Argument("foo", 1.0);
            Argument bar = new Argument("foo", 2.0);
            Expression e = new Expression(ExpressionToUse, foo, bar);
            return e.calculate();
        }
    }
}
