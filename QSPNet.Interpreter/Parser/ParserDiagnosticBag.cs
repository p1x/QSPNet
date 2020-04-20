namespace QSPNet.Interpreter {
    public class ParserDiagnosticBag : DiagnosticBag {
        public const int ParserCode = 2 << 16;
        
        public const int UnexpectedToken = 1;

        public ParserDiagnosticBag() : base(ParserCode) { }

        public void ReportUnexpectedToken(SyntaxKind expectedKind, SyntaxToken actualToken) => 
            Report(UnexpectedToken, actualToken.Position, actualToken.Text, $"Unexpected token '{{0}}' at position {{1}}. Expecting '{expectedKind.GetText()}'.");
    }
}