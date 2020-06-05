namespace QSP.CodeAnalysis {
    public class BoundElementAccessExpression : BoundVariableExpression {
        public BoundElementAccessExpression(VariableSymbol variable, BoundExpression? index) : 
            base(variable, BoundNodeKind.ElementAccessExpression) {
            Index = index;
        }
        
        public BoundExpression? Index { get; }
    }
}