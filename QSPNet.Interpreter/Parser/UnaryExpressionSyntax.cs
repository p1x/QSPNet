using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class UnaryExpressionSyntax : ExpressionSyntax {
        public UnaryExpressionSyntax(SyntaxToken @operator, ExpressionSyntax operand) {
            Operator = @operator;
            Operand = operand;
        }
        
        public SyntaxToken Operator { get; }
        public ExpressionSyntax Operand { get; }
        
        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public override IEnumerable<object> GetChildren() {
            yield return Operator;
            yield return Operand;
        }
    }
}