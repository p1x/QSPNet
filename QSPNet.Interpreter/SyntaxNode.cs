using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public abstract class SyntaxNode {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<object> GetChildren();

        public override string ToString() => Kind.ToString();
    }
}