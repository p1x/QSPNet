namespace QSP.CodeAnalysis {
    public class VariableChangeResult : EvaluationResult {
        public VariableChangeResult(VariableSymbol variable, object result) : base(result) {
            Variable = variable;
        }
        
        public VariableSymbol Variable { get; }
    }
}