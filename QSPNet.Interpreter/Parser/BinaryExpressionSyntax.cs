using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class BinaryExpressionSyntax : ExpressionSyntax {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken @operator, ExpressionSyntax right) {
            
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Operator = @operator ?? throw new ArgumentNullException(nameof(@operator));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Binary;

        public ExpressionSyntax Left { get; }

        public SyntaxToken Operator { get; }

        public ExpressionSyntax Right { get; }

        public override IEnumerable<object> GetChildren() {
            yield return Left;
            yield return Operator;
            yield return Right;
        }
    }
}