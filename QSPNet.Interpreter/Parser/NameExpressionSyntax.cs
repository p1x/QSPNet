using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class NameExpressionSyntax : ExpressionSyntax {
        public NameExpressionSyntax(SyntaxToken identifier) {
            Identifier = identifier;
        }

        public override SyntaxKind Kind => SyntaxKind.NameExpression;
        
        public SyntaxToken Identifier { get; }
        
        public override IEnumerable<object> GetChildren() {
            yield return Identifier;
        }
    }
}