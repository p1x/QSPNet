namespace QSPNet.Interpreter {
    public enum SyntaxTokenKind {
        EndOfFile        = 0x00000000,
        Unknown          = 0x00000001,
        WhiteSpace       = 0x00000002,
        Number           = 0x00000003,
        Plus             = 0x00000004,
        Minus            = 0x00000005,
        Star             = 0x00000006,
        Slash            = 0x00000007,
        OpenParenthesis  = 0x00000008,
        CloseParenthesis = 0x00000009,
    }
    public enum SyntaxExpressionKind {
        Number = 0x00010000,
        Binary = 0x00010001,
    }
    public enum SyntaxKind {
        EndOfFileToken        = 0x00000000,
        UnknownToken          = 0x00000001,
        WhiteSpaceToken       = 0x00000002,
        NumberToken           = 0x00000003,
        PlusToken             = 0x00000004,
        MinusToken            = 0x00000005,
        StarToken             = 0x00000006,
        SlashToken            = 0x00000007,
        OpenParenthesisToken  = 0x00000008,
        CloseParenthesisToken = 0x00000009,
        NumberExpression      = 0x0001000a,
        BinaryExpression      = 0x0001000b,
    }

    public static partial class SyntaxFacts {
        public static bool IsToken(this SyntaxKind kind) => ((int)kind & 0) > 0; 
        public static bool IsExpression(this SyntaxKind kind) => ((int)kind & 65536) > 0; 
        public static SyntaxTokenKind AsToken(this SyntaxKind kind) => (SyntaxTokenKind)(int)kind; 
        public static SyntaxExpressionKind AsExpression(this SyntaxKind kind) => (SyntaxExpressionKind)(int)kind; 
        public static SyntaxKind AsSyntaxKind(this SyntaxTokenKind kind) => (SyntaxKind)(int)kind; 
        public static SyntaxKind AsSyntaxKind(this SyntaxExpressionKind kind) => (SyntaxKind)(int)kind; 
        public static string GetText(this SyntaxKind kind) => GetText(kind.AsToken());  
        public static string GetText(this SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Plus             => "+",
                SyntaxTokenKind.Minus            => "-",
                SyntaxTokenKind.Star             => "*",
                SyntaxTokenKind.Slash            => "/",
                SyntaxTokenKind.OpenParenthesis  => "(",
                SyntaxTokenKind.CloseParenthesis => ")",
                _ => kind.ToString()
            };
        
        public static int GetUnaryPrecedence(this SyntaxKind kind) => GetUnaryPrecedence(kind.AsToken());
        public static int GetUnaryPrecedence(this SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Plus  => 0x00010001,
                SyntaxTokenKind.Minus => 0x00010001,
                _ => 0
            };

        public static int GetBinaryPrecedence(this SyntaxKind kind) => GetBinaryPrecedence(kind.AsToken());
        public static int GetBinaryPrecedence(this SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Plus  => 0x00000001,
                SyntaxTokenKind.Minus => 0x00000001,
                SyntaxTokenKind.Star  => 0x00000002,
                SyntaxTokenKind.Slash => 0x00000002,
                _ => 0
            };
    }

    public static class LexerHelper {
        public static SyntaxKind LexCharacter(char current) 
            => current switch {
                '+' => SyntaxKind.PlusToken,
                '-' => SyntaxKind.MinusToken,
                '*' => SyntaxKind.StarToken,
                '/' => SyntaxKind.SlashToken,
                '(' => SyntaxKind.OpenParenthesisToken,
                ')' => SyntaxKind.CloseParenthesisToken,
                _ => SyntaxKind.UnknownToken
            };
    }
}
