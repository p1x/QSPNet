﻿namespace QSPNet.Interpreter {
    public enum SyntaxTokenKind {
        EndOfFile        = 0x00000000,
        EndOfLine        = 0x00000001,
        Unknown          = 0x00000002,
        WhiteSpace       = 0x00000003,
        Number           = 0x00000004,
        String           = 0x00000005,
        Identifier       = 0x00000006,
        Equals           = 0x00000007,
        Plus             = 0x00000008,
        Minus            = 0x00000009,
        Mod              = 0x0000000a,
        Star             = 0x0000000b,
        Slash            = 0x0000000c,
        OpenParenthesis  = 0x0000000d,
        CloseParenthesis = 0x0000000e,
        ContinueLine     = 0x0000000f,
    }
    public enum SyntaxExpressionKind {
        Literal = 0x00010010,
        Unary   = 0x00010011,
        Binary  = 0x00010012,
        Name    = 0x00010013,
    }
    public enum SyntaxStatementKind {
        Expression = 0x00020014,
        Assignment = 0x00020015,
    }
    public enum SyntaxKind {
        EndOfFileToken        = 0x00000000,
        EndOfLineToken        = 0x00000001,
        UnknownToken          = 0x00000002,
        WhiteSpaceToken       = 0x00000003,
        NumberToken           = 0x00000004,
        StringToken           = 0x00000005,
        IdentifierToken       = 0x00000006,
        EqualsToken           = 0x00000007,
        PlusToken             = 0x00000008,
        MinusToken            = 0x00000009,
        ModToken              = 0x0000000a,
        StarToken             = 0x0000000b,
        SlashToken            = 0x0000000c,
        OpenParenthesisToken  = 0x0000000d,
        CloseParenthesisToken = 0x0000000e,
        ContinueLineToken     = 0x0000000f,
        LiteralExpression     = 0x00010010,
        UnaryExpression       = 0x00010011,
        BinaryExpression      = 0x00010012,
        NameExpression        = 0x00010013,
        ExpressionStatement   = 0x00020014,
        AssignmentStatement   = 0x00020015,
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
                SyntaxTokenKind.Plus   => 0x00010001,
                SyntaxTokenKind.Minus  => 0x00010001,
                _ => 0
            };

        public static Precedence GetBinaryPrecedence(this SyntaxKind kind) => GetBinaryPrecedence(kind.AsToken());
        public static Precedence GetBinaryPrecedence(this SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Equals => 0x00000001,
                SyntaxTokenKind.Plus   => 0x00000002,
                SyntaxTokenKind.Minus  => 0x00000002,
                SyntaxTokenKind.Mod    => 0x00000003,
                SyntaxTokenKind.Star   => 0x00000004,
                SyntaxTokenKind.Slash  => 0x00000004,
                _ => 0
            };
    }
}
