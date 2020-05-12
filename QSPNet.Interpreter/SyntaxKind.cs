﻿namespace QSPNet.Interpreter {
    public enum SyntaxTokenKind {
        EndOfFile        = 0x00000000,
        EndOfLine        = 0x00000001,
        Unknown          = 0x00000002,
        WhiteSpace       = 0x00000003,
        Number           = 0x00000004,
        String           = 0x00000005,
        Identifier       = 0x00000006,
        Ampersand        = 0x00000007,
        Equals           = 0x00000008,
        Plus             = 0x00000009,
        Minus            = 0x0000000a,
        Mod              = 0x0000000b,
        Star             = 0x0000000c,
        Slash            = 0x0000000d,
        OpenParenthesis  = 0x0000000e,
        CloseParenthesis = 0x0000000f,
        ContinueLine     = 0x00000010,
    }
    public enum SyntaxExpressionKind {
        Literal       = 0x00010011,
        Unary         = 0x00010012,
        Binary        = 0x00010013,
        Name          = 0x00010014,
        Parenthesised = 0x00010015,
    }
    public enum SyntaxStatementKind {
        Expression = 0x00020016,
        Assignment = 0x00020017,
    }
    public enum SyntaxKind {
        EndOfFileToken          = 0x00000000,
        EndOfLineToken          = 0x00000001,
        UnknownToken            = 0x00000002,
        WhiteSpaceToken         = 0x00000003,
        NumberToken             = 0x00000004,
        StringToken             = 0x00000005,
        IdentifierToken         = 0x00000006,
        AmpersandToken          = 0x00000007,
        EqualsToken             = 0x00000008,
        PlusToken               = 0x00000009,
        MinusToken              = 0x0000000a,
        ModToken                = 0x0000000b,
        StarToken               = 0x0000000c,
        SlashToken              = 0x0000000d,
        OpenParenthesisToken    = 0x0000000e,
        CloseParenthesisToken   = 0x0000000f,
        ContinueLineToken       = 0x00000010,
        LiteralExpression       = 0x00010011,
        UnaryExpression         = 0x00010012,
        BinaryExpression        = 0x00010013,
        NameExpression          = 0x00010014,
        ParenthesisedExpression = 0x00010015,
        ExpressionStatement     = 0x00020016,
        AssignmentStatement     = 0x00020017,
    }

    public static partial class SyntaxFacts {
        public static bool IsToken(this SyntaxKind kind) => ((int)kind & 0) > 0; 
        public static bool IsExpression(this SyntaxKind kind) => ((int)kind & 65536) > 0; 
        public static bool IsStatement(this SyntaxKind kind) => ((int)kind & 131072) > 0; 
        public static SyntaxTokenKind AsToken(this SyntaxKind kind) => (SyntaxTokenKind)(int)kind; 
        public static SyntaxExpressionKind AsExpression(this SyntaxKind kind) => (SyntaxExpressionKind)(int)kind; 
        public static SyntaxStatementKind AsStatement(this SyntaxKind kind) => (SyntaxStatementKind)(int)kind; 
        public static SyntaxKind AsSyntaxKind(this SyntaxTokenKind kind) => (SyntaxKind)(int)kind; 
        public static SyntaxKind AsSyntaxKind(this SyntaxExpressionKind kind) => (SyntaxKind)(int)kind; 
        public static SyntaxKind AsSyntaxKind(this SyntaxStatementKind kind) => (SyntaxKind)(int)kind; 
        public static string GetText(this SyntaxKind kind) => GetText(kind.AsToken());  
        public static string GetText(this SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Ampersand        => "&",
                SyntaxTokenKind.Equals           => "=",
                SyntaxTokenKind.Plus             => "+",
                SyntaxTokenKind.Minus            => "-",
                SyntaxTokenKind.Mod              => "MOD",
                SyntaxTokenKind.Star             => "*",
                SyntaxTokenKind.Slash            => "/",
                SyntaxTokenKind.OpenParenthesis  => "(",
                SyntaxTokenKind.CloseParenthesis => ")",
                SyntaxTokenKind.ContinueLine     => "_",
                _ => kind.ToString()
            };
        
        public static Precedence GetUnaryPrecedence(this SyntaxKind kind) => GetUnaryPrecedence(kind.AsToken());
        public static Precedence GetUnaryPrecedence(this SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Plus      => 0x00010001,
                SyntaxTokenKind.Minus     => 0x00010001,
                _ => 0
            };

        public static Precedence GetBinaryPrecedence(this SyntaxKind kind) => GetBinaryPrecedence(kind.AsToken());
        public static Precedence GetBinaryPrecedence(this SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Ampersand => 0x00000001,
                SyntaxTokenKind.Equals    => 0x00000002,
                SyntaxTokenKind.Plus      => 0x00000003,
                SyntaxTokenKind.Minus     => 0x00000003,
                SyntaxTokenKind.Mod       => 0x00000004,
                SyntaxTokenKind.Star      => 0x00000005,
                SyntaxTokenKind.Slash     => 0x00000005,
                _ => 0
            };
    }
}
