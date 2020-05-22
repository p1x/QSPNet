namespace QSP.CodeAnalysis {
    public class BoundVariableExpression : BoundExpression {
        public BoundVariableExpression(VariableSymbol variable) : base(BoundNodeKind.VariableExpression) {
            Variable = variable;
        }
        
        protected BoundVariableExpression(VariableSymbol variable, BoundNodeKind kind) : base(kind) {
            Variable = variable;
        }
        
        public VariableSymbol Variable { get; }

        public override BoundType Type => Variable.Type;
    }
}