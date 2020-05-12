namespace QSPNet.Interpreter {
    public class Parser : ParserBase {
        public Parser(string text) : base(text) { }

        protected override StatementSyntax ParseCore() => ParseStatement();

        private StatementSyntax ParseStatement() {
            if (Current.Kind == SyntaxTokenKind.Identifier && Lookahead.Kind == SyntaxTokenKind.Equals)
                return ParseAssignmentStatement();

            return ParseExpressionStatement();
        }

        private StatementSyntax ParseAssignmentStatement() {
            var identifierToken = Match(SyntaxTokenKind.Identifier);
            var equalsToken     = Match(SyntaxTokenKind.Equals);
            var expression      = ParseExpression();
            var endOfLineToken  = Match(SyntaxTokenKind.EndOfLine);
            return new AssignmentStatementSyntax(identifierToken, equalsToken, expression, endOfLineToken);
        }

        private StatementSyntax ParseExpressionStatement() {
            var expression     = ParseExpression();
            var endOfLineToken = Match(SyntaxTokenKind.EndOfLine);
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
        
        private ExpressionSyntax ParsePrimaryExpression() =>
            Current.Kind switch {
                SyntaxTokenKind.OpenParenthesis => ParseParenthesisedExpression(),
                SyntaxTokenKind.Number => new LiteralExpressionSyntax(Match(SyntaxTokenKind.Number)),
                SyntaxTokenKind.String => new LiteralExpressionSyntax(Match(SyntaxTokenKind.String)),
                _ => new NameExpressionSyntax(Match(SyntaxTokenKind.Identifier))
            };

        private ExpressionSyntax ParseParenthesisedExpression() {
            var open = Match(SyntaxTokenKind.OpenParenthesis);
            var expression = ParseExpression();
            var close = Match(SyntaxTokenKind.CloseParenthesis);
            return new ParenthesisedExpressionSyntax(open, expression, close);
        }
    }
}