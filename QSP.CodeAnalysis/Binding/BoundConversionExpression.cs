using System;

namespace QSP.CodeAnalysis {
    public class BoundConversionExpression : BoundExpression {
        public BoundConversionExpression(BoundExpression expression, BoundType type) : base(BoundNodeKind.ConversionExpression) {
            if (type.IsIntermediate)
                throw new ArgumentException("type.IsIntermediate", nameof(type));
            
            Expression = expression;
            Type       = type;
        }
        
        public BoundExpression Expression { get; }
        
        public override BoundType Type { get; }
    }
}