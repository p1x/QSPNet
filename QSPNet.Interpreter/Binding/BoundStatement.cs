namespace QSPNet.Interpreter.Binding {
    public abstract class BoundStatement : BoundNode {
        protected BoundStatement(BoundNodeKind kind) : base(kind) { }
    }
}