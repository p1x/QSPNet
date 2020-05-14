using System;

namespace QSP.CodeAnalysis {
    public class BoundUnaryExpression : BoundExpression {
        public BoundUnaryExpression(BoundUnaryOperator @operator, BoundExpression operand) : base(BoundNodeKind.UnaryExpression) {
            if (operand.Kind == BoundNodeKind.ErrorExpression)
                throw new ArgumentException("operand.Kind == BoundNodeKind.ErrorExpression", nameof(operand));
            if (@operator.Kind == BoundUnaryOperatorKind.Undefined)
                throw new ArgumentException("@operator.Kind == BoundUnaryOperatorKind.Undefined", nameof(@operator));
            
            Operator = @operator;
            Operand = operand;
        }
        
        public BoundUnaryOperator Operator { get; }
        
        public BoundExpression Operand { get; }

        public override BoundType Type => Operator.ResultType;
    }
}