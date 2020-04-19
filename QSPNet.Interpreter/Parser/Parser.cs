namespace QSPNet.Interpreter {
    public class Parser {
        private readonly Lexer _lexer;

        private SyntaxToken _current;
        private SyntaxToken _next;
        
        public Parser(string text) {
            _lexer = new Lexer(text);
            _current = FilterNext(_lexer);
            _next = FilterNext(_lexer);
        }

        private static SyntaxToken FilterNext(Lexer lexer) {
            SyntaxToken token;
            do
                token = lexer.Next();
            while (token.Kind == SyntaxKind.WhiteSpaceToken ||
                   token.Kind == SyntaxKind.Unknown); 

            return token;
        }
        
        private SyntaxToken Current => _current;
        
        private SyntaxToken Next() {
            var current = _current;
            _current = _next;
            _next = FilterNext(_lexer);
            return current;
        }

        private SyntaxToken Match(SyntaxKind kind) {
            if (Current.Kind == kind)
                return Next();

            return SyntaxToken.Manufacture(kind, Current.Position);
        }
        
        public ExpressionSyntax Parse() {
            var left = ParsePrimaryExpression();

            while (Current.Kind == SyntaxKind.PlusToken || 
                   Current.Kind == SyntaxKind.MinusToken) {
                
                var operatorToken = Next();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression() {
            var token = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(token);
        }
    }
}