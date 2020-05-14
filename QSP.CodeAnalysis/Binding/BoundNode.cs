namespace QSP.CodeAnalysis {
    public abstract class BoundNode {
        public BoundNode(BoundNodeKind kind) {
            Kind = kind;
        }
        
        public BoundNodeKind Kind { get; } 
    }
}