namespace QSP.CodeAnalysis {
    public abstract class ExpressionSyntax : SyntaxNode {
        public abstract SyntaxExpressionKind ExpressionKind { get; }

        public override SyntaxKind Kind => ExpressionKind.AsSyntaxKind();
    }
}