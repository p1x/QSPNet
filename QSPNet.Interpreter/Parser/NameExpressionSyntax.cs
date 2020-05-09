using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class NameExpressionSyntax : ExpressionSyntax {
        public NameExpressionSyntax(SyntaxToken identifier) {
            Identifier = identifier;
        }

        public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Name;

        public SyntaxToken Identifier { get; }
        
        public override IEnumerable<object> GetChildren() {
            yield return Identifier;
        }
    }
}