namespace QSPNet.Interpreter {
    public class Evaluator {
        private readonly ExpressionSyntax _expression;

        public Evaluator(ExpressionSyntax expression) {
            _expression = expression;
        }

        public object Evaluate() => EvaluateExpression(_expression);

        private object EvaluateExpression(ExpressionSyntax expression) =>
            expression switch {
                NumberExpressionSyntax n => EvaluateNumberExpression(n),
                BinaryExpressionSyntax b => EvaluateBinaryExpression(b),
                _ => string.Empty
            };

        private object EvaluateBinaryExpression(BinaryExpressionSyntax b) {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);
            return b.Operator.Kind switch {
                SyntaxKind.PlusToken => (int)left + (int)right,
                SyntaxKind.MinusToken => (int)left - (int)right,
                SyntaxKind.SlashToken => (int)left / (int)right,
                SyntaxKind.StarToken => (int)left * (int)right,
                _ => string.Empty
            };
        }

        private static object EvaluateNumberExpression(NumberExpressionSyntax n) {
            return n.Token.Value!;
        }
    }
}