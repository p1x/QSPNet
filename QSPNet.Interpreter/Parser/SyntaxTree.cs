namespace QSPNet.Interpreter {
    public class SyntaxTree {
        public SyntaxTree(string text, StatementSyntax root, SyntaxToken endOfFileToken) {
            Text = text;
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public string Text { get; }
        public StatementSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
    }
}