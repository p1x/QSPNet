namespace QSPNet.Interpreter {
    public class LexerDiagnosticBag : DiagnosticBag {
        public const int LexerCode = 1 << 16;
        
        public const int BadCharacter = 1;

        public LexerDiagnosticBag() : base(LexerCode) { }

        public void ReportBadCharacter(int start, string text) => 
            Report(BadCharacter, start, text, "Bad character '{0}' at {1}.");
    }
}