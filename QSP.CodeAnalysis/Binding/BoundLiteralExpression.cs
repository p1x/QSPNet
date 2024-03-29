﻿using System;

namespace QSP.CodeAnalysis {
    public class BoundLiteralExpression : BoundExpression {
        public BoundLiteralExpression(BoundType type, object value) : base(BoundNodeKind.LiteralExpression) {
            if (type == BoundType.Number && !(value is int) ||
                type == BoundType.String && !(value is string))
                throw new ArgumentException();
            
            Type = type;
            Value = value;
        }
        
        public object Value { get; }
        
        public override BoundType Type { get; }
    }
}