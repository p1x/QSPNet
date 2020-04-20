﻿namespace QSPNet.Interpreter {
    public class Parser {
        private readonly Lexer _lexer;
        private readonly ParserDiagnosticBag _diagnostics = new ParserDiagnosticBag();
        
        private SyntaxToken _current;
        private SyntaxToken _next;
        
        public Parser(string text) {
            _lexer = new Lexer(text);
            _current = FilterNext(_lexer);
            _next = FilterNext(_lexer);
        }

        public (ExpressionSyntax syntax, DiagnosticBag diagnostics) Parse() {
            var left = ParsePrimaryExpression();

            while (_current.Kind == SyntaxKind.PlusToken || 
                   _current.Kind == SyntaxKind.MinusToken) {
                
                var operatorToken = Next();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            var diagnostics = _lexer.GetDiagnostics().With(_diagnostics);
            return (left, diagnostics);
        }

        private ExpressionSyntax ParsePrimaryExpression() {
            var token = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(token);
        }

        private static SyntaxToken FilterNext(Lexer lexer) {
            SyntaxToken token;
            do
                token = lexer.Next();
            while (token.Kind == SyntaxKind.WhiteSpaceToken ||
                   token.Kind == SyntaxKind.Unknown); 

            return token;
        }

        private SyntaxToken Next() {
            var current = _current;
            _current = _next;
            _next = FilterNext(_lexer);
            return current;
        }

        private SyntaxToken Match(SyntaxKind kind) {
            if (_current.Kind == kind)
                return Next();

            _diagnostics.ReportUnexpectedToken(kind, _current);
            return SyntaxToken.Manufacture(kind, _current.Position);
        }
    }
}