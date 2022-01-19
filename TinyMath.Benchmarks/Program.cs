using BenchmarkDotNet.Running;
using TinyMath.Benchmarks;

var summary = BenchmarkRunner.Run<ExpressionEvaluationBenchmark>();

Console.ReadLine();

