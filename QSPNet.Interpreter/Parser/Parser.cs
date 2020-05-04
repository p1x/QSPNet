namespace QSPNet.Interpreter {
    public class Parser : ParserBase {
        public Parser(string text) : base(text) { }

        protected override ExpressionSyntax ParseCore() => ParseExpression();

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0) {
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