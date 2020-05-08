﻿namespace QSPNet.Interpreter {
    public enum SyntaxTokenKind {
        EndOfFile        = 0x00000000,
        EndOfLine        = 0x00000001,
        Unknown          = 0x00000002,
        WhiteSpace       = 0x00000003,
        Number           = 0x00000004,
        Identifier       = 0x00000005,
        Equals           = 0x00000006,
        Plus             = 0x00000007,
        Minus            = 0x00000008,
        Mod              = 0x00000009,
        Star             = 0x0000000a,
        Slash            = 0x0000000b,
        OpenParenthesis  = 0x0000000c,
        CloseParenthesis = 0x0000000d,
        ContinueLine     = 0x0000000e,
    }
    public enum SyntaxExpressionKind {
        Number = 0x00010000,
        Unary  = 0x00010001,
        Binary = 0x00010002,
        Name   = 0x00010003,
    }
    public enum SyntaxStatementKind {
        Expression = 0x00020000,
        Assignment = 0x00020001,
    }
    public enum SyntaxKind {
        EndOfFileToken        = 0x00000000,
        EndOfLineToken        = 0x00000001,
        UnknownToken          = 0x00000002,
        WhiteSpaceToken       = 0x00000003,
        NumberToken           = 0x00000004,
        IdentifierToken       = 0x00000005,
        EqualsToken           = 0x00000006,
        PlusToken             = 0x00000007,
        MinusToken            = 0x00000008,
        ModToken              = 0x00000009,
        StarToken             = 0x0000000a,
        SlashToken            = 0x0000000b,
        OpenParenthesisToken  = 0x0000000c,
        CloseParenthesisToken = 0x0000000d,
        ContinueLineToken     = 0x0000000e,
        NumberExpression      = 0x0001000f,
        UnaryExpression       = 0x00010010,
        BinaryExpression      = 0x00010011,
        NameExpression        = 0x00010012,
        ExpressionStatement   = 0x00020013,
        AssignmentStatement   = 0x00020014,
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
