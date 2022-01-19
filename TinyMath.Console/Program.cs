// See https://aka.ms/new-console-template for more information
using TinyMath;

try
{
    var expr = ExpressionParser.Parse("2 ^ 3 ^ 2");
    var result = expr.Evaluate(new DictionaryExpressionContext
    {
        {"foo",1.0m },
        {"bar", 2.0m }
    });

    Console.WriteLine(result);
    
}
catch(ParserException e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.Position);
}
Console.ReadLine();