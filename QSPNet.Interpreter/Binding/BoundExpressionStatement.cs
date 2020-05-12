using System;

namespace QSPNet.Interpreter.Binding {
    public class BoundExpressionStatement : BoundStatement {
        public BoundExpressionStatement(BoundExpression expression) : base(BoundNodeKind.ExpressionStatement) {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }
        
        public BoundExpression Expression { get; }
    }
}