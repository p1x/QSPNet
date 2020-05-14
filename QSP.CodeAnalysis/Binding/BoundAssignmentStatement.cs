using System;

namespace QSP.CodeAnalysis {
    public class BoundAssignmentStatement : BoundStatement {
        public BoundAssignmentStatement(VariableSymbol variable, BoundExpression expression) : base(BoundNodeKind.AssignmentStatement) {
            if (expression.Kind == BoundNodeKind.ErrorExpression)
                throw new ArgumentException("expression.Kind == BoundNodeKind.ErrorExpression", nameof(expression));
            
            Variable = variable;
            Expression = expression;
        }
        
        public VariableSymbol Variable { get; }
        
        public BoundExpression Expression { get; }
    }
}