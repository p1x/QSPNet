namespace QSP.CodeAnalysis {
    public class BoundBinaryOperator {

        private BoundBinaryOperator(BoundBinaryOperatorKind kind, BoundType type) : this(kind, type, type, type) { }

        private BoundBinaryOperator(BoundBinaryOperatorKind kind, BoundType leftType, BoundType rightType, BoundType returnType) {
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ReturnType = returnType;
        }
        public BoundBinaryOperatorKind Kind { get; }
        public BoundType LeftType { get; }
        public BoundType RightType { get; }
        public BoundType ReturnType { get; }
        
        private static readonly BoundBinaryOperator Undefined = new BoundBinaryOperator(BoundBinaryOperatorKind.Undefined, BoundType.Undefined);
        private static readonly BoundBinaryOperator Addition = new BoundBinaryOperator(BoundBinaryOperatorKind.Addition, BoundType.Number);
        private static readonly BoundBinaryOperator Subtraction = new BoundBinaryOperator(BoundBinaryOperatorKind.Subtraction, BoundType.Number);
        private static readonly BoundBinaryOperator Multiplication = new BoundBinaryOperator(BoundBinaryOperatorKind.Multiplication, BoundType.Number);
        private static readonly BoundBinaryOperator Division = new BoundBinaryOperator(BoundBinaryOperatorKind.Division, BoundType.Number);
        private static readonly BoundBinaryOperator Modulus = new BoundBinaryOperator(BoundBinaryOperatorKind.Modulus, BoundType.Number);
        
        private static readonly BoundBinaryOperator Concatenation = new BoundBinaryOperator(BoundBinaryOperatorKind.Concatenation, BoundType.Any, BoundType.Any, BoundType.String);
        private static readonly BoundBinaryOperator DynamicAddition = new BoundBinaryOperator(BoundBinaryOperatorKind.DynamicAddition, BoundType.Any);
        
        public static BoundBinaryOperator Bind(SyntaxTokenKind operatorKind, BoundType leftType, BoundType rightType) =>
            operatorKind switch {
                SyntaxTokenKind.Plus       when leftType == BoundType.Number && rightType == BoundType.Number => Addition,
                SyntaxTokenKind.Minus      when leftType == BoundType.Number && rightType == BoundType.Number => Subtraction,
                SyntaxTokenKind.Star       when leftType == BoundType.Number && rightType == BoundType.Number => Multiplication,
                SyntaxTokenKind.Slash      when leftType == BoundType.Number && rightType == BoundType.Number => Division,
                SyntaxTokenKind.Mod        when leftType == BoundType.Number && rightType == BoundType.Number => Modulus,
                
                SyntaxTokenKind.Plus       when leftType == BoundType.String && rightType == BoundType.String => Concatenation,
                SyntaxTokenKind.Ampersand                                                                     => Concatenation,
                
                SyntaxTokenKind.Plus       when leftType == BoundType.String && rightType == BoundType.String => Concatenation,
                SyntaxTokenKind.Plus                                                                          => DynamicAddition,
                
                _ => Undefined
            };
    }
}