﻿namespace QSPNet.Interpreter {
    public enum SyntaxTokenKind {
        EndOfFile        = 0x00000000,
        Unknown          = 0x00000001,
        WhiteSpace       = 0x00000002,
        Number           = 0x00000003,
        Plus             = 0x00000004,
        Minus            = 0x00000005,
        Mod              = 0x00000006,
        Star             = 0x00000007,
        Slash            = 0x00000008,
        OpenParenthesis  = 0x00000009,
        CloseParenthesis = 0x0000000a,
    }
    public enum SyntaxExpressionKind {
        Number = 0x00010000,
        Unary  = 0x00010001,
        Binary = 0x00010002,
    }
    public enum SyntaxKind {
        EndOfFileToken        = 0x00000000,
        UnknownToken          = 0x00000001,
        WhiteSpaceToken       = 0x00000002,
        NumberToken           = 0x00000003,
        PlusToken             = 0x00000004,
        MinusToken            = 0x00000005,
        ModToken              = 0x00000006,
        StarToken             = 0x00000007,
        SlashToken            = 0x00000008,
        OpenParenthesisToken  = 0x00000009,
        CloseParenthesisToken = 0x0000000a,
        NumberExpression      = 0x0001000b,
        UnaryExpression       = 0x0001000c,
        BinaryExpression      = 0x0001000d,
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
                SyntaxTokenKind.Mod              => "MOD",
                SyntaxTokenKind.Star             => "*",
                SyntaxTokenKind.Slash            => "/",
                SyntaxTokenKind.OpenParenthesis  => "(",
                SyntaxTokenKind.CloseParenthesis => ")",
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
