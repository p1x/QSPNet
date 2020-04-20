﻿using System;
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
        private char Peek() => _position < _text.Length ? _text[_position] : Char.Null;
        
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

            if (tryRead(char.IsDigit))
                return new SyntaxToken(SyntaxKind.NumberToken, start, _text.Substring(start, _position - start));
            if (tryRead(char.IsWhiteSpace))
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, _text.Substring(start, _position - start));
            
            NextChar(); // consume one
            switch (current) {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, start, _text.Substring(start, 1));
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, start, _text.Substring(start, 1));
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, start, _text.Substring(start, 1));
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, start, _text.Substring(start, 1));
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, start, _text.Substring(start, 1));
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, start, _text.Substring(start, 1));
                default:
                    var tokenText = _text.Substring(start, 1);
                    _diagnostics.ReportUnknownToken(start, tokenText);
                    return new SyntaxToken(SyntaxKind.Unknown, start, tokenText);
            }
        }
    }
}