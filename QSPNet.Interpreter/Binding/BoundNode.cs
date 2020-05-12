namespace QSPNet.Interpreter.Binding {
    public abstract class BoundNode {
        public BoundNode(BoundNodeKind kind) {
            Kind = kind;
        }
        
        public BoundNodeKind Kind { get; } 
    }
}