using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class UnaryExpressionSyntax : ExpressionSyntax {
        public UnaryExpressionSyntax(SyntaxToken @operator, ExpressionSyntax operand) {
            Operator = @operator ?? throw new ArgumentNullException(nameof(@operator));
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));
        }

        public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Unary;
        
        public SyntaxToken Operator { get; }
        
        public ExpressionSyntax Operand { get; }

        public override IEnumerable<object> GetChildren() {
            yield return Operator;
            yield return Operand;
        }
    }
}