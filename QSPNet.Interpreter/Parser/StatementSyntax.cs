using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public abstract class StatementSyntax : SyntaxNode {
        protected StatementSyntax(SyntaxToken endOfLineToken) {
            EndOfLineToken = endOfLineToken ?? throw new ArgumentNullException(nameof(endOfLineToken));
        }

        public override SyntaxKind Kind => StatementKind.AsSyntaxKind();

        public abstract SyntaxStatementKind StatementKind { get; }
        
        public SyntaxToken EndOfLineToken { get; }

        public override IEnumerable<object> GetChildren() {
            yield return EndOfLineToken;
        }
    }
}