using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace QSP.CodeAnalysis {
    public class Lexer {
        private readonly LexerDiagnosticBag _diagnostics = new LexerDiagnosticBag();
        private readonly string _text;
        private int _position;
        private SyntaxToken? _currentToken;
        
        public Lexer(string text) {
            _text = text;
        }

        public IEnumerable<SyntaxToken> Lex() {
            SyntaxToken token;
            while ((token = Next()).Kind != SyntaxTokenKind.EndOfFile)
                yield return token;
        }

        public LexerDiagnosticBag GetDiagnostics() => _diagnostics;

        [Pure]
        private char Peek(int ahead = 0) => _position + ahead < _text.Length ? _text[_position + ahead] : Char.Null;

        public SyntaxToken Next() => _currentToken = NextToken();

        private SyntaxToken NextToken() {
            var current = Peek();
            switch (current) {
                case Char.Null:
                    return ConsumeNullChar();
                case '+':
                    return ConsumeSingleCharToken(SyntaxTokenKind.Plus, _position);
                case '-':
                    return ConsumeSingleCharToken(SyntaxTokenKind.Minus, _position);
                case '*':
                    return ConsumeSingleCharToken(SyntaxTokenKind.Star, _position);
                case '/':
                    return ConsumeSingleCharToken(SyntaxTokenKind.Slash, _position);
                case '=':
                    return ConsumeSingleCharToken(SyntaxTokenKind.Equals, _position);
                case '&':
                    return ConsumeSingleCharToken(SyntaxTokenKind.Ampersand, _position);
                case '(':
                    return ConsumeSingleCharToken(SyntaxTokenKind.OpenParenthesis, _position);
                case ')':
                    return ConsumeSingleCharToken(SyntaxTokenKind.CloseParenthesis, _position);
                case 'M': case 'm':
                    return TryConsumeModOperator(_position) ?? ConsumeIdentifierOrKeyword(_position);
                case '_':
                    return TryConsumeContinueLineToken(_position) ?? ConsumeIdentifierOrKeyword(_position);
                case '\r':
                    return TryConsumeEndOfLineToken(_position) ?? ConsumeBadCharacter(_position);
                case ' ':
                    return ConsumeWhiteSpaceToken(_position);
                case '0': case '1': case '2': case '3': case '4':
                case '5': case '6': case '7': case '8': case '9':
                    return ConsumeNumberToken(_position);
                case '\'': case '"':
                    return ConsumeStringToken(_position);
                default:
                    return char.IsLetter(current) || current == '$'
                        ? ConsumeIdentifierOrKeyword(_position)
                        : ConsumeBadCharacter(_position);
            }
        }

        private SyntaxToken ConsumeNullChar() {
            _position++;
            return new SyntaxToken(SyntaxTokenKind.EndOfFile, _text.Length, Char.NullString);
        }

        private SyntaxToken ConsumeSingleCharToken(SyntaxTokenKind kind, int start) {
            _position++;
            return new SyntaxToken(kind, start, GetCurrentTokenText(start));
        }

        private SyntaxToken ConsumeWhiteSpaceToken(int start) {
            do
                _position++;
            while (_position < _text.Length && _text[_position] == ' ');
            
            return new SyntaxToken(SyntaxTokenKind.WhiteSpace, start, GetCurrentTokenText(start));
        }

        private SyntaxToken ConsumeNumberToken(int start) {
            do
                _position++;
            while (_position < _text.Length && char.IsDigit(_text[_position]));
            
            var tokenText = GetCurrentTokenText(start);
            if (int.TryParse(tokenText, out var value))
                return new SyntaxToken(SyntaxTokenKind.Number, start, tokenText, value);

            _diagnostics.ReportInvalidInteger(start, tokenText);
            return SyntaxToken.Manufacture(SyntaxTokenKind.Number, start);
        }

        private SyntaxToken ConsumeStringToken(int start) {
            var startChar = _text[_position];
            var builder = new StringBuilder();
            
            while (true) {
                _position++;

                if (_position >= _text.Length) {
                    _diagnostics.ReportUnterminatedString(start, GetCurrentTokenText(start));
                    break;
                }

                if (_text[_position] == startChar) {
                    _position++;
                    if (_text[_position] == startChar)
                        builder.Append(startChar);
                    else
                        break;
                } else {
                    builder.Append(_text[_position]);
                }
            }
            
            var value = builder.ToString();
            return new SyntaxToken(SyntaxTokenKind.String, start, GetCurrentTokenText(start), value);
        }

        private SyntaxToken ConsumeIdentifierOrKeyword(int start) {
            do
                _position++;
            while (_position < _text.Length && (char.IsLetter(_text[_position]) || _text[_position] == '_'));

            var text = GetCurrentTokenText(start).ToUpperInvariant();
            var kind = text switch {
                "INPUT" => SyntaxTokenKind.InputFunc,
                _       => SyntaxTokenKind.Identifier
            };
            
            return new SyntaxToken(kind, start, GetCurrentTokenText(start));
        }

        private SyntaxToken? TryConsumeModOperator(int start) {
            if (Peek(1) != 'O' && Peek(1) != 'o' ||
                Peek(2) != 'D' && Peek(2) != 'd')
                return null;
            
            _position += 3;
            return new SyntaxToken(SyntaxTokenKind.Mod, start, GetCurrentTokenText(start));
        }

        private SyntaxToken? TryConsumeContinueLineToken(int start) {
            if (_currentToken?.Text != " " || Peek(1) != '\r' || Peek(2) != '\n')
                return null;
            
            _position += 3;
            return new SyntaxToken(SyntaxTokenKind.ContinueLine, start, GetCurrentTokenText(start));
        }

        private SyntaxToken? TryConsumeEndOfLineToken(int start) {
            if (Peek(1) != '\n')
                return null;
            
            _position += 2;
            return new SyntaxToken(SyntaxTokenKind.EndOfLine, start, GetCurrentTokenText(start));
        }

        private SyntaxToken ConsumeBadCharacter(int start) {
            _position++;
            var tokenText = GetCurrentTokenText(start);
            _diagnostics.ReportBadCharacter(start, tokenText);
            return new SyntaxToken(SyntaxTokenKind.Unknown, start, tokenText);
        }

        private string GetCurrentTokenText(int start) => _text.Substring(start, _position - start);
    }
}