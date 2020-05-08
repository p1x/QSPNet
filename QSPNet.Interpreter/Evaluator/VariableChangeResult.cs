namespace QSPNet.Interpreter {
    public class VariableChangeResult : EvaluationResult {
        public VariableChangeResult(string variableName, object result) : base(result) {
            VariableName = variableName;
        }
        
        public string VariableName { get; }
    }
}