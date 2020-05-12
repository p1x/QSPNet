using System.Collections.Immutable;

namespace QSPNet.Interpreter {
    public class SyntaxTree {
        public SyntaxTree(string text, ImmutableArray<StatementSyntax> statements, SyntaxToken endOfFileToken) {
            Text = text;
            Statements = statements;
            EndOfFileToken = endOfFileToken;
        }

        public string Text { get; }
        
        public ImmutableArray<StatementSyntax> Statements { get; }
        
        public SyntaxToken EndOfFileToken { get; }
    }
}