﻿namespace QSPNet.Interpreter {
    public static partial class SyntaxFacts {
        public static bool HasValue(this SyntaxKind kind) => kind == SyntaxKind.NumberToken;
        public static bool HasValue(this SyntaxTokenKind kind) => kind == SyntaxTokenKind.Number;
    }
}