namespace QSPNet.Interpreter.Binding {
    public class BoundBinaryExpression : BoundExpression {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator @operator, BoundExpression right) : base(BoundNodeKind.BinaryExpression) {
            Left = left;
            Operator = @operator;
            Right = right;
        }
        
        public BoundExpression Left { get; }
        
        public BoundBinaryOperator Operator { get; }
        
        public BoundExpression Right { get; }

        public override BoundType Type => Operator.ReturnType;
    }
}