using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public class Interpreter
    {
        public Interpreter(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        private Tokenizer _tokenizer;

        public IExpression ParseExpression()
        {
            var expr = ParseAddSubtract();

            // check everything was consumed
            if (_tokenizer.CurrentToken != Token.EOF)
                throw new ParserException("Unexpected characters at end of expression", _tokenizer.ToString(), _tokenizer.CurrentPosition-1);

            return expr;
        }

       

        private IExpression ParseAddSubtract()
        {
            var lhs = ParseMultiplyDivide();

            while (true)
            {
                char op = Constants.NullChar;
                switch (_tokenizer.CurrentToken)
                {
                    case Token.Add: op = '+'; break;
                    case Token.Subtract: op = '-'; break;
                    default: return lhs;
                }

                _tokenizer.NextToken();

                var rhs = ParseMultiplyDivide();

                lhs = new BinaryExpression(lhs, rhs, op);
            }
        }

        private IExpression ParseMultiplyDivide()
        {
            var lhs = ParsePow();

            while (true)
            {
                char op = Constants.NullChar;
                switch (_tokenizer.CurrentToken)
                {
                    case Token.Multiply: op = '*'; break;
                    case Token.Divide: op = '/'; break;
                    default: return lhs;
                }

                _tokenizer.NextToken();

                var rhs = ParsePow();

                lhs = new BinaryExpression(lhs, rhs, op);
            }
        }

        private IExpression ParsePow()
        {
            var lhs = ParseUnary();

            while (true)
            {
                char op = Constants.NullChar;
                switch (_tokenizer.CurrentToken)
                {
                    case Token.Pow: op = '^'; break;
                    default: return lhs;
                }

                _tokenizer.NextToken();

                var rhs = ParseAddSubtract();

                lhs = new BinaryExpression(lhs, rhs, op);
            }
        }

        private IExpression ParseUnary()
        {
            while (true)
            {
                // positive op is a no-op
                if(_tokenizer.CurrentToken == Token.Add)
                {
                    _tokenizer.NextToken();
                    continue;
                }

                // negative op
                if(_tokenizer.CurrentToken == Token.Subtract)
                {
                    // skip
                    _tokenizer.NextToken();

                    var rhs = ParseUnary();

                    return new UnaryExpression(rhs , Constants.Subtract);
                }

                return ParseLeaf();
            }
        }

        private IExpression ParseLeaf()
        {
            if(_tokenizer.CurrentToken == Token.Number)
            {
                var expr = new ConstantExpression(_tokenizer.Number);
                _tokenizer.NextToken();
                return expr;
            }

            if(_tokenizer.CurrentToken == Token.OpenParens)
            {
                // skip '('
                _tokenizer.NextToken();

                // parse top level
                var expr = ParseAddSubtract();

                // check and skip ')'
                if(_tokenizer.CurrentToken != Token.CloseParens)
                    throw new ParserException("Expected closed parens", _tokenizer.ToString(), _tokenizer.CurrentPosition-1);

                _tokenizer.NextToken();

                return expr;
            }

            if(_tokenizer.CurrentToken == Token.Identifier)
            {
                //var name = new ReadOnlyMemory<char>(_tokenizer.Identifier.ToArray());
                var name = _tokenizer.Identifier.ToString();
                _tokenizer.NextToken();

                //parens indicate a method call
                if(_tokenizer.CurrentToken != Token.OpenParens)
                {
                    return new VariableExpression(name);
                }
                else
                {
                    // skip parens
                    _tokenizer.NextToken();

                    var args = new List<IExpression>();
                    while (true)
                    {
                        args.Add(ParseAddSubtract());

                        if(_tokenizer.CurrentToken == Token.Comma)
                        {
                            _tokenizer.NextToken();
                            continue;
                        }

                        break;
                    }

                    if(_tokenizer.CurrentToken != Token.CloseParens)
                    {
                        throw new ParserException("Expected closed parens", _tokenizer.ToString(), _tokenizer.CurrentPosition-1);
                    }

                    _tokenizer.NextToken();

                    return new FunctionCallExpression(name, args);
                }

            }

            throw new ParserException($"Unexpected token '{_tokenizer.CurrentToken}'", _tokenizer.ToString(), _tokenizer.CurrentPosition-1);
        }
    }
}
