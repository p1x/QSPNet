using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class NumberExpressionSyntax : ExpressionSyntax {
        public NumberExpressionSyntax(SyntaxToken token) {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
        
        public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Number;
        
        public SyntaxToken Token { get; }

        public override IEnumerable<object> GetChildren() {
            yield return Token;
        }
    }
}