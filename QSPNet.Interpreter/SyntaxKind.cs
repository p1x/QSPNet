﻿namespace QSPNet.Interpreter {
    public enum SyntaxTokenKind {
        EndOfFile        = 0x00000000,
        EndOfLine        = 0x00000001,
        Unknown          = 0x00000002,
        WhiteSpace       = 0x00000003,
        Number           = 0x00000004,
        Identifier       = 0x00000005,
        Plus             = 0x00000006,
        Minus            = 0x00000007,
        Mod              = 0x00000008,
        Star             = 0x00000009,
        Slash            = 0x0000000a,
        OpenParenthesis  = 0x0000000b,
        CloseParenthesis = 0x0000000c,
        ContinueLine     = 0x0000000d,
    }
    public enum SyntaxExpressionKind {
        Number = 0x00010000,
        Unary  = 0x00010001,
        Binary = 0x00010002,
        Name   = 0x00010003,
    }
    public enum SyntaxStatementKind {
        Expression = 0x00020000,
    }
    public enum SyntaxKind {
        EndOfFileToken        = 0x00000000,
        EndOfLineToken        = 0x00000001,
        UnknownToken          = 0x00000002,
        WhiteSpaceToken       = 0x00000003,
        NumberToken           = 0x00000004,
        IdentifierToken       = 0x00000005,
        PlusToken             = 0x00000006,
        MinusToken            = 0x00000007,
        ModToken              = 0x00000008,
        StarToken             = 0x00000009,
        SlashToken            = 0x0000000a,
        OpenParenthesisToken  = 0x0000000b,
        CloseParenthesisToken = 0x0000000c,
        ContinueLineToken     = 0x0000000d,
        NumberExpression      = 0x0001000e,
        UnaryExpression       = 0x0001000f,
        BinaryExpression      = 0x00010010,
        NameExpression        = 0x00010011,
        ExpressionStatement   = 0x00020012,
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
                SyntaxTokenKind.Plus  => 0x00010001,
                SyntaxTokenKind.Minus => 0x00010001,
                _ => 0
            };

        public static Precedence GetBinaryPrecedence(this SyntaxKind kind) => GetBinaryPrecedence(kind.AsToken());
        public static Precedence GetBinaryPrecedence(this SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Plus  => 0x00000001,
                SyntaxTokenKind.Minus => 0x00000001,
                SyntaxTokenKind.Mod   => 0x00000002,
                SyntaxTokenKind.Star  => 0x00000003,
                SyntaxTokenKind.Slash => 0x00000003,
                _ => 0
            };
    }
}
