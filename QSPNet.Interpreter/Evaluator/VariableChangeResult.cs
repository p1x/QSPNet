using QSPNet.Interpreter.Binding;

namespace QSPNet.Interpreter {
    public class VariableChangeResult : EvaluationResult {
        public VariableChangeResult(VariableSymbol variable, object result) : base(result) {
            Variable = variable;
        }
        
        public VariableSymbol Variable { get; }
    }
}