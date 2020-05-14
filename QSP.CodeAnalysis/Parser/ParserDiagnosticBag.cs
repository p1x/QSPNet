namespace QSP.CodeAnalysis {
    public class ParserDiagnosticBag : DiagnosticBag {
        public const int ParserCode = 2 << 16;
        
        public const int UnexpectedToken = 1;
        public const int UnexpectedEndOfStatementToken = 2;

        public ParserDiagnosticBag() : base(ParserCode) { }

        public void ReportUnexpectedToken(SyntaxTokenKind expectedKind, SyntaxToken actualToken) => 
            Report(UnexpectedToken, actualToken.Position, actualToken.Text, $"Unexpected token '{{0}}' at position {{1}}. Expecting '{expectedKind.GetText()}'.");

        public void ReportUnexpectedEndOfStatementToken(SyntaxToken actualToken) {
            Report(UnexpectedEndOfStatementToken, actualToken.Position, actualToken.Text, $"Unexpected token '{{0}}' at position {{1}}. Expecting '{SyntaxTokenKind.EndOfLine.GetText()}' or {SyntaxTokenKind.Ampersand.GetText()}.");
        }
    }
}