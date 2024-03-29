﻿namespace QSP.CodeAnalysis {
    public abstract class BoundExpression : BoundNode {
        public BoundExpression(BoundNodeKind kind) : base(kind) { }

        public abstract BoundType Type { get; }
    }
}