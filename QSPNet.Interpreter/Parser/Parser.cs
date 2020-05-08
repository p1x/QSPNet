namespace QSPNet.Interpreter {
    public class Parser : ParserBase {
        public Parser(string text) : base(text) { }

        protected override StatementSyntax ParseCore() => ParseStatement();

        private StatementSyntax ParseStatement() {
            if (Current.Kind == SyntaxKind.IdentifierToken && Lookahead.Kind == SyntaxKind.EqualsToken)
                return ParseAssignmentStatement();

            return ParseExpressionStatement();
        }

        private StatementSyntax ParseAssignmentStatement() {
            var identifierToken = Match(SyntaxKind.IdentifierToken);
            var equalsToken     = Match(SyntaxKind.EqualsToken);
            var expression      = ParseExpression();
            var endOfLineToken  = Match(SyntaxKind.EndOfLineToken);
            return new AssignmentStatementSyntax(identifierToken, equalsToken, expression, endOfLineToken);
        }

        private StatementSyntax ParseExpressionStatement() {
            var expression     = ParseExpression();
            var endOfLineToken = Match(SyntaxKind.EndOfLineToken);
            return new ExpressionStatementSyntax(expression, endOfLineToken);
        }

        private ExpressionSyntax ParseExpression(Precedence parentPrecedence = default) {
            var left = ParseExpressionLeft(parentPrecedence);

            Precedence precedence;
            while ((precedence = Current.Kind.GetBinaryPrecedence()) > parentPrecedence) {
                var operatorToken = Next();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParseExpressionLeft(Precedence parentPrecedence) {
            // if current is unary operator (precedence > 0) and its precedence > parentPrecedence
            var unaryPrecedence = Current.Kind.GetUnaryPrecedence();
            if (unaryPrecedence > parentPrecedence) {
                var operatorToken = Next();
                var operand = ParseExpression(unaryPrecedence);
                return new UnaryExpressionSyntax(operatorToken, operand);
            }

            return ParsePrimaryExpression();
        }
        
        private ExpressionSyntax ParsePrimaryExpression() {
            if (Current.Kind == SyntaxKind.NumberToken)
                return new NumberExpressionSyntax(Match(SyntaxKind.NumberToken));

            return new NameExpressionSyntax(Match(SyntaxKind.IdentifierToken));
        }
    }
}