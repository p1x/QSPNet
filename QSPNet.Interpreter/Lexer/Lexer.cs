using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class Lexer {
        private readonly LexerDiagnosticBag _diagnostics = new LexerDiagnosticBag();
        private readonly string _text;
        private int _position;
        
        public Lexer(string text) {
            _text = text;
        }

        public IEnumerable<SyntaxToken> Lex() {
            SyntaxToken token;
            while ((token = Next()).Kind != SyntaxKind.EndOfFileToken)
                yield return token;
        }

        public LexerDiagnosticBag GetDiagnostics() => _diagnostics; 
        
        private char NextChar() {
            _position++;
            return Peek();
        }
        private char Peek(int ahead = 0) => _position + ahead < _text.Length ? _text[_position + ahead] : Char.Null;
        private char Lookahead() => Peek(1);
        
        public SyntaxToken Next() {
            if (_position >= _text.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _text.Length, Char.NullString);
            
            var start = _position;
            var current = Peek();

            bool tryRead(Predicate<char> predicate) {
                if (!predicate(current))
                    return false;
                
                do
                    current = NextChar();
                while (predicate(current) && !Char.IsNull(current));
                return true;
            }

            if (tryRead(char.IsDigit)) {
                var tokenText = _text.Substring(start, _position - start);
                if (int.TryParse(tokenText, out var value))
                    return new SyntaxToken(SyntaxKind.NumberToken, start, tokenText, value);

                _diagnostics.ReportInvalidInteger(start, tokenText);
                return SyntaxToken.Manufacture(SyntaxKind.NumberToken, start);
            }
            if (tryRead(char.IsWhiteSpace))
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, _text.Substring(start, _position - start));

            
            SyntaxToken token;
            switch (current) {
                case '+': token = new SyntaxToken(SyntaxKind.PlusToken, start, _text.Substring(start, 1)); break;
                case '-': token = new SyntaxToken(SyntaxKind.MinusToken, start, _text.Substring(start, 1)); break;
                case '*': token = new SyntaxToken(SyntaxKind.StarToken, start, _text.Substring(start, 1)); break;
                case '/': token = new SyntaxToken(SyntaxKind.SlashToken, start, _text.Substring(start, 1)); break;
                case '(': token = new SyntaxToken(SyntaxKind.OpenParenthesisToken, start, _text.Substring(start, 1)); break;
                case ')': token = new SyntaxToken(SyntaxKind.CloseParenthesisToken, start, _text.Substring(start, 1)); break;
                case 'M': case 'm':
                    if (Lookahead() == 'O' || Lookahead() == 'o') {
                        NextChar();
                        if (Lookahead() == 'D' || Lookahead() == 'd') {
                            NextChar();
                            token = new SyntaxToken(SyntaxKind.ModToken, start, _text.Substring(start, 3));
                            break;
                        }
                    }
                    goto default;
                default: token = LexBadCharacter(start); break;
            }
            NextChar(); // consume any
            return token;
        }

        private SyntaxToken LexBadCharacter(int start) {
            var tokenText = _text.Substring(start, 1);
            _diagnostics.ReportBadCharacter(start, tokenText);
            return new SyntaxToken(SyntaxKind.UnknownToken, start, tokenText);
        }
    }
}