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
        private static readonly BoundBinaryOperator Addition = new BoundBinaryOperator(BoundBinaryOperatorKind.Addition, BoundType.Integer);
        private static readonly BoundBinaryOperator Subtraction = new BoundBinaryOperator(BoundBinaryOperatorKind.Subtraction, BoundType.Integer);
        private static readonly BoundBinaryOperator Multiplication = new BoundBinaryOperator(BoundBinaryOperatorKind.Multiplication, BoundType.Integer);
        private static readonly BoundBinaryOperator Division = new BoundBinaryOperator(BoundBinaryOperatorKind.Division, BoundType.Integer);
        private static readonly BoundBinaryOperator Modulus = new BoundBinaryOperator(BoundBinaryOperatorKind.Modulus, BoundType.Integer);
        
        private static readonly BoundBinaryOperator Concatenation = new BoundBinaryOperator(BoundBinaryOperatorKind.Concatenation, BoundType.String);
        private static readonly BoundBinaryOperator DynamicAddition1 = new BoundBinaryOperator(BoundBinaryOperatorKind.DynamicAddition, BoundType.Integer, BoundType.String, BoundType.Dynamic);
        private static readonly BoundBinaryOperator DynamicAddition2 = new BoundBinaryOperator(BoundBinaryOperatorKind.DynamicAddition, BoundType.String, BoundType.Integer, BoundType.Dynamic);
        
        public static BoundBinaryOperator Bind(SyntaxTokenKind operatorKind, BoundType leftType, BoundType rightType) =>
            operatorKind switch {
                SyntaxTokenKind.Plus       when leftType == BoundType.Integer && rightType == BoundType.Integer => Addition,
                SyntaxTokenKind.Minus      when leftType == BoundType.Integer && rightType == BoundType.Integer => Subtraction,
                SyntaxTokenKind.Star       when leftType == BoundType.Integer && rightType == BoundType.Integer => Multiplication,
                SyntaxTokenKind.Slash      when leftType == BoundType.Integer && rightType == BoundType.Integer => Division,
                SyntaxTokenKind.Mod        when leftType == BoundType.Integer && rightType == BoundType.Integer => Modulus,
                
                SyntaxTokenKind.Plus       when leftType == BoundType.String  && rightType == BoundType.String  => Concatenation,
                
                SyntaxTokenKind.Ampersand  when leftType == BoundType.String  && rightType == BoundType.String  => Concatenation,
                SyntaxTokenKind.Ampersand  when leftType == BoundType.Integer && rightType == BoundType.String  => Concatenation,
                SyntaxTokenKind.Ampersand  when leftType == BoundType.String  && rightType == BoundType.Integer => Concatenation,
                SyntaxTokenKind.Ampersand  when leftType == BoundType.Integer && rightType == BoundType.Integer => Concatenation,
                
                SyntaxTokenKind.Plus       when leftType == BoundType.Integer && rightType == BoundType.String  => DynamicAddition1,
                SyntaxTokenKind.Plus       when leftType == BoundType.String  && rightType == BoundType.Integer => DynamicAddition2,
                
                _ => Undefined
            };
    }
}