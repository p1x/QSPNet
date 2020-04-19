namespace QSPNet.Interpreter {
    public class SyntaxToken {
        public SyntaxToken(SyntaxKind kind, int position, string text) {
            Kind = kind;
            Position = position;
            Text = text;
        }

        public override string ToString() => $"{Kind}:{Position} '{Text}'";

        public SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
    }
}