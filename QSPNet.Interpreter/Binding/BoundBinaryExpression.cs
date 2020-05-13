using System;

namespace QSPNet.Interpreter.Binding {
    public class BoundBinaryExpression : BoundExpression {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator @operator, BoundExpression right) : base(BoundNodeKind.BinaryExpression) {
            if (left.Kind == BoundNodeKind.ErrorExpression)
                throw new ArgumentException("left.Kind == BoundNodeKind.ErrorExpression", nameof(left));
            if (right.Kind == BoundNodeKind.ErrorExpression)
                throw new ArgumentException("right.Kind == BoundNodeKind.ErrorExpression", nameof(right));
            if (@operator.Kind == BoundBinaryOperatorKind.Undefined)
                throw new ArgumentException("@operator.Kind == BoundBinaryOperatorKind.Undefined", nameof(@operator));
            
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