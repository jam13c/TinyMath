using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TinyMath
{
    public class Tokenizer
    {

        static Regex IsNumber = new Regex(@"[0-9.]");
        static Regex IsLetterOrUnderscore = new Regex("[0-9a-zA-Z_]");

        public Tokenizer(string expr)
        {
            _expr = expr;
            NextToken();
        }

        private string _expr = String.Empty;
        private int _currentPos = 0;
        private Token _currentToken = Token.EOF;
        private decimal _number = 0m;
        private string _identifier = String.Empty;

        public Token CurrentToken => _currentToken;
        public decimal Number => _number;
        public string Identifier => _identifier;
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
            var currentChar = _currentPos < _expr.Length ? _expr.Substring(_currentPos,1) : String.Empty;
            // skip whitespace
            while (currentChar != String.Empty && string.IsNullOrWhiteSpace(currentChar))
            {
                NextChar();
                currentChar = _currentPos < _expr.Length ? _expr.Substring(_currentPos, 1) : String.Empty;
            }

            switch(currentChar)
            {
                case "":
                    _currentToken = Token.EOF;
                    return;

                case "+":
                    NextChar();
                    _currentToken = Token.Add;
                    return;

                case "-":
                    NextChar();
                    _currentToken = Token.Subtract;
                    return;

                case "*":
                    NextChar();
                    _currentToken = Token.Multiply;
                    return;

                case "/":
                    NextChar();
                    _currentToken = Token.Divide;
                    return;

                case "^":
                    NextChar();
                    _currentToken = Token.Pow;
                    return;

                case "(":
                    NextChar();
                    _currentToken = Token.OpenParens;
                    return;

                case ")":
                    NextChar();
                    _currentToken = Token.CloseParens;
                    return;

                case ",":
                    NextChar();
                    _currentToken = Token.Comma;
                    return;

            }

            if(IsNumber.IsMatch(currentChar))
            {
                bool hasDecimal = false;
                int len = 0;
                while(IsNumber.IsMatch(currentChar) || (!hasDecimal && currentChar == "."))
                {                    
                    len++;
                    if (_currentPos + len == _expr.Length)
                        break;
                    hasDecimal = currentChar == ".";
                    currentChar = _expr.Substring(_currentPos + len,1);
                }
                _number = decimal.Parse(_expr.Substring(_currentPos, len));
                _currentToken = Token.Number;
                Advance(len);
                return;
            }

            if(IsLetterOrUnderscore.IsMatch(currentChar))
            {
                var len = 0;
                while(IsLetterOrUnderscore.IsMatch(currentChar))
                {                   
                    len++;
                    if (_currentPos + len == _expr.Length)
                        break;
                    currentChar = _expr.Substring(_currentPos + len,1);
                }

                _identifier = _expr.Substring(_currentPos, len);
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
