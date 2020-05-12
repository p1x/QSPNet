namespace QSPNet.Interpreter.Binding {
    public class BoundErrorExpression : BoundExpression {
        public static BoundErrorExpression Instance { get; } = new BoundErrorExpression();
        
        public BoundErrorExpression() : base(BoundNodeKind.ErrorExpression) { }
        
        public override BoundType Type => BoundType.Undefined;
    }
}