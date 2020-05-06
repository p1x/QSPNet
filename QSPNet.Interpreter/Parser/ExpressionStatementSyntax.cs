using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class ExpressionStatementSyntax : StatementSyntax {
        public ExpressionStatementSyntax(ExpressionSyntax expression, SyntaxToken endOfLineToken) : base(endOfLineToken) {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override SyntaxKind Kind { get; } = SyntaxKind.ExpressionStatement;
        
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<object> GetChildren() {
            yield return Expression;
            foreach (var child in base.GetChildren())
                yield return child;
        }
    }
}