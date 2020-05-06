using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public abstract class StatementSyntax : SyntaxNode {
        protected StatementSyntax(SyntaxToken endOfLineToken) {
            EndOfLineToken = endOfLineToken ?? throw new ArgumentNullException(nameof(endOfLineToken));
        }
        
        public SyntaxToken EndOfLineToken { get; }

        public override IEnumerable<object> GetChildren() {
            yield return EndOfLineToken;
        }
    }
}