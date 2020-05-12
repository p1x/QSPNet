using System;

namespace QSPNet.Interpreter.Binding {
    public class BoundUnaryExpression : BoundExpression {
        public BoundUnaryExpression(BoundUnaryOperator @operator, BoundExpression operand) : base(BoundNodeKind.UnaryExpression) {
            Operator = @operator ?? throw new ArgumentNullException(nameof(@operator));
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));
        }
        
        public BoundUnaryOperator Operator { get; }
        
        public BoundExpression Operand { get; }

        public override BoundType Type => Operator.ResultType;
    }
}