namespace QSPNet.Interpreter {
    public class EvaluationResult {
        public EvaluationResult(object value) {
            Value = value;
        }
        
        public object Value { get; } 
    }
}