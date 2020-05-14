namespace QSP.CodeAnalysis {
    public class BoundExpressionStatement : BoundStatement {
        public BoundExpressionStatement(BoundExpression expression) : base(BoundNodeKind.ExpressionStatement) {
            Expression = expression;
        }
        
        public BoundExpression Expression { get; }
    }
}