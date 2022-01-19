using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public interface IExpression
    {
        decimal Evaluate(IExpressionContext ctx);
    }
}
