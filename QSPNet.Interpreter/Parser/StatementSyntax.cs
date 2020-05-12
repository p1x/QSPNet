using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public abstract class StatementSyntax : SyntaxNode {
        protected StatementSyntax(SyntaxToken endToken) {
            EndToken = endToken ?? throw new ArgumentNullException(nameof(endToken));
        }

        public override SyntaxKind Kind => StatementKind.AsSyntaxKind();

        public abstract SyntaxStatementKind StatementKind { get; }
        
        public SyntaxToken EndToken { get; }

        public override IEnumerable<object> GetChildren() {
            yield return EndToken;
        }
    }
}