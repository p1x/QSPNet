namespace QSPNet.Interpreter {
    public class SyntaxTree {
        public SyntaxTree(string text, ExpressionSyntax root, SyntaxToken endOfFileToken) {
            Text = text;
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public string Text { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
    }
}