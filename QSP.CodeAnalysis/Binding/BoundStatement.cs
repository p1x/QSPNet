namespace QSP.CodeAnalysis {
    public abstract class BoundStatement : BoundNode {
        protected BoundStatement(BoundNodeKind kind) : base(kind) { }
    }
}