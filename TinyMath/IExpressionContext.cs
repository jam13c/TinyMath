namespace TinyMath
{
    public interface IExpressionContext
    {
        decimal ResolveVariable(string key);
        decimal ExecuteFunction(string key, decimal[] arguments);
    }
}
