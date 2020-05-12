﻿namespace QSPNet.Interpreter {
    public enum SyntaxTokenKind {
        EndOfFile        = 0x00010000,
        EndOfLine        = 0x00010001,
        Unknown          = 0x00010002,
        WhiteSpace       = 0x00010003,
        Number           = 0x00010004,
        String           = 0x00010005,
        Identifier       = 0x00010006,
        Ampersand        = 0x00010007,
        Equals           = 0x00010008,
        Plus             = 0x00010009,
        Minus            = 0x0001000a,
        Mod              = 0x0001000b,
        Star             = 0x0001000c,
        Slash            = 0x0001000d,
        OpenParenthesis  = 0x0001000e,
        CloseParenthesis = 0x0001000f,
        ContinueLine     = 0x00010010,
    }
    public enum SyntaxExpressionKind {
        Literal       = 0x00020011,
        Unary         = 0x00020012,
        Binary        = 0x00020013,
        Name          = 0x00020014,
        Parenthesised = 0x00020015,
    }
    public enum SyntaxStatementKind {
        Expression = 0x00030016,
        Assignment = 0x00030017,
    }
    public enum SyntaxCustomKind {
        CompilationUnit = 0x00000018,
    }
    public enum SyntaxKind {
        EndOfFileToken          = 0x00010000,
        EndOfLineToken          = 0x00010001,
        UnknownToken            = 0x00010002,
        WhiteSpaceToken         = 0x00010003,
        NumberToken             = 0x00010004,
        StringToken             = 0x00010005,
        IdentifierToken         = 0x00010006,
        AmpersandToken          = 0x00010007,
        EqualsToken             = 0x00010008,
        PlusToken               = 0x00010009,
        MinusToken              = 0x0001000a,
        ModToken                = 0x0001000b,
        StarToken               = 0x0001000c,
        SlashToken              = 0x0001000d,
        OpenParenthesisToken    = 0x0001000e,
        CloseParenthesisToken   = 0x0001000f,
        ContinueLineToken       = 0x00010010,
        LiteralExpression       = 0x00020011,
        UnaryExpression         = 0x00020012,
        BinaryExpression        = 0x00020013,
        NameExpression          = 0x00020014,
        ParenthesisedExpression = 0x00020015,
        ExpressionStatement     = 0x00030016,
        AssignmentStatement     = 0x00030017,
        CompilationUnit         = 0x00000018,
    }

    public static partial class SyntaxFacts {
        public static bool IsCustom(this SyntaxKind kind) => ((int)kind & 0) > 0; 
        public static bool IsToken(this SyntaxKind kind) => ((int)kind & 65536) > 0; 
        public static bool IsExpression(this SyntaxKind kind) => ((int)kind & 131072) > 0; 
        public static bool IsStatement(this SyntaxKind kind) => ((int)kind & 196608) > 0; 
        public static SyntaxCustomKind AsCustom(this SyntaxKind kind) => (SyntaxCustomKind)(int)kind; 
        public static SyntaxTokenKind AsToken(this SyntaxKind kind) => (SyntaxTokenKind)(int)kind; 
        public static SyntaxExpressionKind AsExpression(this SyntaxKind kind) => (SyntaxExpressionKind)(int)kind; 
        public static SyntaxStatementKind AsStatement(this SyntaxKind kind) => (SyntaxStatementKind)(int)kind; 
        public static SyntaxKind AsSyntaxKind(this SyntaxCustomKind kind) => (SyntaxKind)(int)kind; 
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
