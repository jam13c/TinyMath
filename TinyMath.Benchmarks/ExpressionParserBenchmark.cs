using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using static TinyMath.Tokenizer;

namespace TinyMath.Benchmarks
{
    [MemoryDiagnoser]
    public class ExpressionParserBenchmark
    {
        static readonly string expr = "3 + 4 * bar / (foo - 5 ) ^ 2 ^ 3";
        [Benchmark]
        public IExpression Evaluator()
        {
            return ExpressionParser.Parse(expr);

        }
    }
}
