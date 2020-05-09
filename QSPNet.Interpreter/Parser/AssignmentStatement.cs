using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class AssignmentStatementSyntax : StatementSyntax {
        public AssignmentStatementSyntax(
            SyntaxToken identifierToken,
            SyntaxToken equalsToken,
            ExpressionSyntax expression,
            SyntaxToken endOfLineToken
        ) : base(endOfLineToken) {
            
            IdentifierToken = identifierToken  ?? throw new ArgumentNullException(nameof(identifierToken));
            EqualsToken = equalsToken  ?? throw new ArgumentNullException(nameof(equalsToken));
            Expression = expression  ?? throw new ArgumentNullException(nameof(expression));
        }

        public override SyntaxStatementKind StatementKind => SyntaxStatementKind.Assignment;
        
        public SyntaxToken IdentifierToken { get; }
        
        public SyntaxToken EqualsToken { get; }
        
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<object> GetChildren() {
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Expression;
            foreach (var child in base.GetChildren())
                yield return child;
        }
    }
}