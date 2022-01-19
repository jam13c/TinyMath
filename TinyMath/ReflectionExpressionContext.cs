using System;
using System.Reflection;

namespace TinyMath
{
    public class ReflectionExpressionContext : IExpressionContext
    {
        private readonly object target;

        public ReflectionExpressionContext(object target)
        {
            this.target = target;
        }

        public decimal ExecuteFunction(string key, decimal[] arguments)
        {
            var method = this.target.GetType().GetMethod(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (method == null)
                throw new InvalidOperationException($"Method '{key}' not found");
            if (method.GetParameters().Length != arguments.Length)
                throw new InvalidOperationException($"Method '{key}' must take {arguments.Length} arguments");
            if(method.ReturnType != typeof(decimal))
                throw new InvalidOperationException($"Method '{key}' must return a decimal");
            return (decimal)method.Invoke(target, arguments.Select(x => (object)x).ToArray())!;
            
        }

        public decimal ResolveVariable(string key)
        {
            var property = this.target.GetType().GetProperty(key,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null)
                throw new InvalidOperationException($"Property '{key}' not found");
            if(property.PropertyType != typeof(decimal))
                throw new InvalidOperationException($"Property '{key}' must return a decimal");
            return (decimal)property.GetValue(this.target)!;
        }
    }
}
