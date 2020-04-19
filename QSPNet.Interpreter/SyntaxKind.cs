namespace QSPNet.Interpreter {
    public enum SyntaxKind {
        // tokens
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
        
        // expressions
        NumberExpression,
        BinaryExpression
    }
}