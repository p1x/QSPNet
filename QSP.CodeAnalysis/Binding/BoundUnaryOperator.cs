﻿namespace QSP.CodeAnalysis {
    public class BoundUnaryOperator {
        private BoundUnaryOperator(BoundUnaryOperatorKind kind, BoundType operandType, BoundType resultType) {
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }
        
        public BoundUnaryOperatorKind Kind { get; }
        
        public BoundType OperandType { get; }
        
        public BoundType ResultType { get; }

        private static readonly BoundUnaryOperator Negation  = new BoundUnaryOperator(BoundUnaryOperatorKind.Negation,  BoundType.Number,   BoundType.Number);
        private static readonly BoundUnaryOperator Identity  = new BoundUnaryOperator(BoundUnaryOperatorKind.Identity,  BoundType.Number,   BoundType.Number);
        private static readonly BoundUnaryOperator Undefined = new BoundUnaryOperator(BoundUnaryOperatorKind.Undefined, BoundType.Undefined, BoundType.Undefined);
        
        public static BoundUnaryOperator Bind(SyntaxTokenKind operatorKind, BoundType operandType) =>
            operatorKind switch {
                SyntaxTokenKind.Plus  => Identity,
                SyntaxTokenKind.Minus => Negation,
                _                     => Undefined
            };
    }
}