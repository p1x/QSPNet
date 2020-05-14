namespace QSP.CodeAnalysis {
    public class BoundVariableExpression : BoundExpression {
        public BoundVariableExpression(VariableSymbol variable) : base(BoundNodeKind.VariableExpression) {
            Variable = variable;
        }
        
        public VariableSymbol Variable { get; }

        public override BoundType Type => Variable.Type;
    }
}