using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace QSPNet.Interpreter {
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
            while ((token = Next()).Kind != SyntaxKind.EndOfFileToken)
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
                    return ConsumeSingleCharToken(SyntaxKind.PlusToken, _position);
                case '-':
                    return ConsumeSingleCharToken(SyntaxKind.MinusToken, _position);
                case '*':
                    return ConsumeSingleCharToken(SyntaxKind.StarToken, _position);
                case '/':
                    return ConsumeSingleCharToken(SyntaxKind.SlashToken, _position);
                case '=':
                    return ConsumeSingleCharToken(SyntaxKind.EqualsToken, _position);
                case '(':
                    return ConsumeSingleCharToken(SyntaxKind.OpenParenthesisToken, _position);
                case ')':
                    return ConsumeSingleCharToken(SyntaxKind.CloseParenthesisToken, _position);
                case 'M': case 'm':
                    return TryConsumeModOperator(_position) ?? ConsumeIdentifier(_position);
                case '_':
                    return TryConsumeContinueLineToken(_position) ?? ConsumeIdentifier(_position);
                case ' ':
                    return ConsumeWhiteSpaceToken(_position);
                case '\r':
                    return TryConsumeEndOfLineToken(_position) ?? ConsumeBadCharacter(_position);
                case '0': case '1': case '2': case '3': case '4':
                case '5': case '6': case '7': case '8': case '9':
                    return ConsumeNumberToken(_position);
                default:
                    return char.IsLetter(current) 
                        ? ConsumeIdentifier(_position)
                        : ConsumeBadCharacter(_position);
            }
        }

        private SyntaxToken ConsumeNullChar() {
            _position++;
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _text.Length, Char.NullString);
        }

        private SyntaxToken ConsumeSingleCharToken(SyntaxKind kind, int start) {
            _position++;
            return new SyntaxToken(kind, start, GetCurrentTokenText(start));
        }

        private SyntaxToken ConsumeWhiteSpaceToken(int start) {
            do
                _position++;
            while (_position < _text.Length && _text[_position] == ' ');
            
            return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, GetCurrentTokenText(start));
        }

        private SyntaxToken ConsumeNumberToken(int start) {
            do
                _position++;
            while (_position < _text.Length && char.IsDigit(_text[_position]));
            
            var tokenText = GetCurrentTokenText(start);
            if (int.TryParse(tokenText, out var value))
                return new SyntaxToken(SyntaxKind.NumberToken, start, tokenText, value);

            _diagnostics.ReportInvalidInteger(start, tokenText);
            return SyntaxToken.Manufacture(SyntaxKind.NumberToken, start);
        }

        private SyntaxToken ConsumeIdentifier(int start) {
            do
                _position++;
            while (_position < _text.Length && (char.IsLetter(_text[_position]) || _text[_position] == '_'));
            
            return new SyntaxToken(SyntaxKind.IdentifierToken, start, GetCurrentTokenText(start));
        }

        private SyntaxToken? TryConsumeModOperator(int start) {
            if (Peek(1) != 'O' && Peek(1) != 'o' ||
                Peek(2) != 'D' && Peek(2) != 'd')
                return null;
            
            _position += 3;
            return new SyntaxToken(SyntaxKind.ModToken, start, GetCurrentTokenText(start));
        }

        private SyntaxToken? TryConsumeContinueLineToken(int start) {
            if (_currentToken?.Text != " " || Peek(1) != '\r' || Peek(2) != '\n')
                return null;
            
            _position += 3;
            return new SyntaxToken(SyntaxKind.ContinueLineToken, start, GetCurrentTokenText(start));
        }

        private SyntaxToken? TryConsumeEndOfLineToken(int start) {
            if (Peek(1) != '\n')
                return null;
            
            _position += 2;
            return new SyntaxToken(SyntaxKind.EndOfLineToken, start, GetCurrentTokenText(start));
        }

        private SyntaxToken ConsumeBadCharacter(int start) {
            _position++;
            var tokenText = GetCurrentTokenText(start);
            _diagnostics.ReportBadCharacter(start, tokenText);
            return new SyntaxToken(SyntaxKind.UnknownToken, start, tokenText);
        }

        private string GetCurrentTokenText(int start) => _text.Substring(start, _position - start);
    }
}