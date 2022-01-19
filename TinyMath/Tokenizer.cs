using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMath
{
    public ref struct Tokenizer
    {        

        public Tokenizer(ReadOnlySpan<char> expr)
        {
            _expr = expr;
            NextToken();
        }

        private ReadOnlySpan<char> _expr = ReadOnlySpan<char>.Empty;
        private int _currentPos = 0;
        private Token _currentToken = Token.EOF;
        private decimal _number = 0m;
        private ReadOnlySpan<char> _identifier = ReadOnlySpan<char>.Empty;

        public Token CurrentToken => _currentToken;
        public decimal Number => _number;
        public ReadOnlySpan<char> Identifier => _identifier;
        public int CurrentPosition => _currentPos;

        public override string ToString() => _expr.ToString();

        void NextChar()
        {
            _currentPos++;
        }

        void Advance(int by)
        {
            _currentPos += by;
        }
        

        public void NextToken()
        {
            var currentChar = _currentPos < _expr.Length ? _expr[_currentPos] : Constants.NullChar;
            // skip whitespace
            while (char.IsWhiteSpace(currentChar))
            {
                NextChar();
                currentChar = _currentPos < _expr.Length ? _expr[_currentPos] : Constants.NullChar;
            }

            switch(currentChar)
            {
                case Constants.NullChar:
                    _currentToken = Token.EOF;
                    return;

                case Constants.Add:
                    NextChar();
                    _currentToken = Token.Add;
                    return;

                case Constants.Subtract:
                    NextChar();
                    _currentToken = Token.Subtract;
                    return;

                case Constants.Multiply:
                    NextChar();
                    _currentToken = Token.Multiply;
                    return;

                case Constants.Divide:
                    NextChar();
                    _currentToken = Token.Divide;
                    return;

                case Constants.Pow:
                    NextChar();
                    _currentToken = Token.Pow;
                    return;

                case Constants.LeftParen:
                    NextChar();
                    _currentToken = Token.OpenParens;
                    return;

                case Constants.RightParen:
                    NextChar();
                    _currentToken = Token.CloseParens;
                    return;

                case Constants.Comma:
                    NextChar();
                    _currentToken = Token.Comma;
                    return;

            }

            if(char.IsDigit(currentChar) || currentChar == '.')
            {
                bool hasDecimal = false;
                int len = 0;
                while(char.IsDigit(currentChar) || (!hasDecimal && currentChar == Constants.DecimalPoint))
                {                    
                    len++;
                    if (_currentPos + len == _expr.Length)
                        break;
                    hasDecimal = currentChar == Constants.DecimalPoint;
                    currentChar = _expr[_currentPos + len];
                }
                _number = decimal.Parse(_expr.Slice(_currentPos, len));
                _currentToken = Token.Number;
                Advance(len);
                return;
            }

            if(char.IsLetter(currentChar) || currentChar == Constants.Underscore)
            {
                var len = 0;
                while(char.IsLetter(currentChar) || char.IsNumber(currentChar) || currentChar == Constants.Underscore)
                {                   
                    len++;
                    if (_currentPos + len == _expr.Length)
                        break;
                    currentChar = _expr[_currentPos + len];
                }

                _identifier = _expr.Slice(_currentPos, len);
                _currentToken = Token.Identifier;
                Advance(len);
                return;
            }

            throw new ParserException($"Unrecognised character '{currentChar}'", _expr.ToString(), _currentPos);
        }

    }

    public enum Token
    {
        EOF,
        Add,
        Subtract,
        Multiply,
        Divide,
        Pow,
        OpenParens,
        CloseParens,
        Comma,
        Identifier,
        Number,
    }
}
