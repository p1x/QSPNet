namespace QSP.CodeAnalysis {
    public class BoundErrorStatement : BoundStatement {
        private BoundErrorStatement() : base(BoundNodeKind.ErrorStatement) { }
        
        public static BoundErrorStatement Instance { get; } = new BoundErrorStatement();
    }
}