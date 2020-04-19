using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class BinaryExpressionSyntax : ExpressionSyntax {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken @operator, ExpressionSyntax right) {
            Left = left;
            Operator = @operator;
            Right = right;
        }
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public override IEnumerable<object> GetChildren() {
            yield return Left;
            yield return Operator;
            yield return Right;
        }

        public ExpressionSyntax Left { get; }
        public SyntaxToken Operator { get; }
        public ExpressionSyntax Right { get; }
    }
}