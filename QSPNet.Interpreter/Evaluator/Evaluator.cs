using System;

namespace QSPNet.Interpreter {
    public class Evaluator {
        private readonly StatementSyntax _statement;

        public Evaluator(StatementSyntax expression) {
            _statement = expression;
        }

        public object Evaluate() =>
            _statement switch {
                ExpressionStatementSyntax e => EvaluateExpressionStatement(e),
                _ => throw new ArgumentOutOfRangeException()
            };

        private object EvaluateExpressionStatement(ExpressionStatementSyntax syntax) =>
            EvaluateExpression(syntax.Expression);
        
        private object EvaluateExpression(ExpressionSyntax expression) =>
            expression switch {
                NumberExpressionSyntax n => EvaluateNumberExpression(n),
                BinaryExpressionSyntax b => EvaluateBinaryExpression(b),
                UnaryExpressionSyntax  u => EvaluateUnaryExpression(u),
                _ => string.Empty
            };

        private object EvaluateUnaryExpression(UnaryExpressionSyntax u) =>
            u.Operator.Kind switch {
                SyntaxKind.PlusToken => EvaluateExpression(u.Operand),
                SyntaxKind.MinusToken => -(int)EvaluateExpression(u.Operand),
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
                SyntaxKind.ModToken => (int)left % (int)right,
                _ => string.Empty
            };
        }

        private static object EvaluateNumberExpression(NumberExpressionSyntax n) => 
            n.Token.Value!;
    }
}