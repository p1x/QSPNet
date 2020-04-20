namespace QSPNet.Interpreter {
    public class LexerDiagnosticBag : DiagnosticBag {
        public const int LexerCode = 1 << 16;
        
        public const int UnexpectedToken = 1;
        
        protected override void Report(int errorCode, int position, string text, string message) => 
            base.Report(errorCode ^ LexerCode, position, text, message);

        public void ReportUnknownToken(int start, string text) => 
            Report(UnexpectedToken, start, text, "Unexpected token '{0}' at {1}.");
    }
}