using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public class DictionaryExpressionContext : IExpressionContext, IEnumerable<KeyValuePair<string, decimal>>
    {
        private readonly IDictionary<string,decimal> values = new Dictionary<string,decimal>();

        public DictionaryExpressionContext()
        {

        }
        public DictionaryExpressionContext(IDictionary<string, decimal> values)
        {
            this.values = values;
        }

        public void Add(string key, decimal value) => values.Add(key, value);

        public decimal ExecuteFunction(string key, decimal[] arguments)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, decimal>> GetEnumerator() => values.GetEnumerator();

        public decimal ResolveVariable(string key)
        {
            if (!values.TryGetValue(key, out decimal result))
                throw new EvaluationException($"Missing variable '{key}'");
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
