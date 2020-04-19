using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class NumberExpressionSyntax : ExpressionSyntax {
        public NumberExpressionSyntax(SyntaxToken token) {
            Token = token;
        }
        
        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        
        public override IEnumerable<object> GetChildren() {
            yield return Token;
        }

        public SyntaxToken Token { get; }
    }
}