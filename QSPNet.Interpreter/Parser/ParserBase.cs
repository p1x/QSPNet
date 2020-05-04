namespace QSPNet.Interpreter {
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
        
        public (SyntaxTree syntaxTree, DiagnosticBag diagnostics) Parse() {
            var expression = ParseCore();
            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);
            var syntaxTree = new SyntaxTree(_text, expression, endOfFileToken);
            var diagnostics = _lexer.GetDiagnostics().With(_diagnostics);
            return (syntaxTree, diagnostics);
        }

        protected abstract ExpressionSyntax ParseCore();

        private static SyntaxToken FilterNext(Lexer lexer) {
            SyntaxToken token;
            do
                token = lexer.Next();
            while (token.Kind == SyntaxKind.WhiteSpaceToken); 

            return token;
        }

        protected SyntaxToken Next() {
            var current = _current;
            _current = _next;
            _next = FilterNext(_lexer);
            return current;
        }

        protected SyntaxToken Match(SyntaxKind kind) {
            if (_current.Kind == kind)
                return Next();

            _diagnostics.ReportUnexpectedToken(kind, _current);
            return SyntaxToken.Manufacture(kind, _current.Position);
        }

        protected SyntaxToken Current => _current;
    }
}