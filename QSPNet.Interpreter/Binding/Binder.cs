using System;

namespace QSPNet.Interpreter.Binding {
    public class Binder {
        private BinderDiagnosticBag _diagnostics = new BinderDiagnosticBag();
        
        public BoundStatement BindStatement(StatementSyntax statement) =>
            statement switch {
                AssignmentStatementSyntax x => BindAssignmentStatement(x),
                ExpressionStatementSyntax x => BindExpressionStatement(x),
                _ => throw new ArgumentOutOfRangeException(nameof(statement))
            };

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax statement) {
            var expression = BindExpression(statement.Expression);
            return new BoundExpressionStatement(expression);
        }

        private BoundStatement BindAssignmentStatement(AssignmentStatementSyntax statement) {
            throw new System.NotImplementedException();
        }

        private BoundExpression BindExpression(ExpressionSyntax expression) =>
            expression switch {
                LiteralExpressionSyntax       x => BindLiteralExpression(x),
                NameExpressionSyntax          x => BindNameExpression(x),
                UnaryExpressionSyntax         x => BindUnaryExpression(x),
                BinaryExpressionSyntax        x => BindBinaryExpression(x),
                ParenthesisedExpressionSyntax x => BindParenthesisedExpression(x),
                _ => throw new ArgumentOutOfRangeException(nameof(expression))
            };

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax expression) {
            var type = BindType(expression.Token.Kind);
            var value = expression.Token.Value ?? throw new ArgumentException("Literal expression should contain value.");
            return new BoundLiteralExpression(type, value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax expression) {
            throw new NotImplementedException();
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax expression) {
            var operand = BindExpression(expression.Operand);
            var @operator = BoundUnaryOperator.Bind(expression.Operator.Kind, operand.Type);

            if (@operator.Kind == BoundUnaryOperatorKind.Undefined) {
                _diagnostics.ReportUndefinedUnaryOperator();
                return operand;
            }
            
            return new BoundUnaryExpression(@operator, operand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax expression) {
            var left = BindExpression(expression.Left);
            var right = BindExpression(expression.Right);
            var @operator = BoundBinaryOperator.Bind(expression.Operator.Kind, left.Type, right.Type);

            if (@operator.Kind == BoundBinaryOperatorKind.Undefined) {
                _diagnostics.ReportUndefinedBinaryOperator();
                return left;
            }
            
            return new BoundBinaryExpression(left, @operator, right);
        }

        private BoundExpression BindParenthesisedExpression(ParenthesisedExpressionSyntax expression) => 
            BindExpression(expression.Expression);

        private static BoundType BindType(SyntaxTokenKind tokenKind) =>
            tokenKind switch {
                SyntaxTokenKind.Number => BoundType.Integer,
                SyntaxTokenKind.String => BoundType.String,
                _                      => BoundType.Undefined
            };
    }
}