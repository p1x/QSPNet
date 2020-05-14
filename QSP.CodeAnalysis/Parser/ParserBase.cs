namespace QSP.CodeAnalysis {
    public abstract class ParserBase {
        private readonly string _text;
        private readonly Lexer _lexer;
        
        private readonly ParserDiagnosticBag _diagnostics = new ParserDiagnosticBag();

        private SyntaxToken _current;
        private SyntaxToken _next;

        protected ParserBase(string text) {
            _text = text;
            _lexer = new Lexer(text);
            _current = FilterNext(_lexer);
            _next = FilterNext(_lexer);
        }
        
        public SyntaxTree Parse() {
            var compilationUnit = ParseCore();
            var diagnostics = _lexer.GetDiagnostics().With(_diagnostics);
            var syntaxTree = new SyntaxTree(_text, compilationUnit, diagnostics);
            return syntaxTree;
        }

        protected abstract CompilationUnitSyntax ParseCore();

        private static SyntaxToken FilterNext(Lexer lexer) {
            SyntaxToken token;
            do
                token = lexer.Next();
            while (token.Kind == SyntaxTokenKind.WhiteSpace ||
                   token.Kind == SyntaxTokenKind.ContinueLine); 

            return token;
        }

        protected SyntaxToken Next() {
            var current = _current;
            _current = _next;
            _next = FilterNext(_lexer);
            return current;
        }

        protected SyntaxToken Match(SyntaxTokenKind kind) {
            if (_current.Kind == kind)
                return Next();

            _diagnostics.ReportUnexpectedToken(kind, _current);
            return SyntaxToken.Manufacture(kind, _current.Position);
        }

        protected SyntaxToken MatchEndOfStatement() {
            if(_current.Kind == SyntaxTokenKind.EndOfLine ||
               _current.Kind == SyntaxTokenKind.Ampersand)
                return Next();
            
            _diagnostics.ReportUnexpectedEndOfStatementToken(_current);
            return SyntaxToken.Manufacture(SyntaxTokenKind.EndOfLine, _current.Position);
        }

        protected SyntaxToken Current => _current;
        protected SyntaxToken Lookahead => _next;
    }
}