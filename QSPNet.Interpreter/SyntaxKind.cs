namespace QSPNet.Interpreter {
    public enum SyntaxKind {
        EndOfFileToken,
        Unknown,

        WhiteSpaceToken,
        NumberToken,
        
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
    }
}