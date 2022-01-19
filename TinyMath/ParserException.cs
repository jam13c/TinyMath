using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{

    [Serializable]
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
            Expression = String.Empty;
        }
        public ParserException(string message, string expression, int position) : base(message)
        {
            Expression = expression;
            Position = position;
        }
        public ParserException(string message, Exception inner, string expression, int position) : base(message, inner)
        {
            Expression = expression;
            Position = position;
        }
        public string Expression { get; }
        public int Position { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Message);
            sb.AppendLine(Expression);
            sb.AppendFormat("{0}^", new string('-',Position));
            return sb.ToString();
        }
    }
}
