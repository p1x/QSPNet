using System;

namespace QSPNet.Interpreter {
    public static class SyntaxFacts {
        public static string GetText(this SyntaxKind kind) =>
            kind switch {
                SyntaxKind.WhiteSpaceToken => " ",
                SyntaxKind.PlusToken => "+",
                SyntaxKind.MinusToken => "-",
                SyntaxKind.StarToken => "*",
                SyntaxKind.SlashToken => "/",
                SyntaxKind.OpenParenthesisToken => "(",
                SyntaxKind.CloseParenthesisToken => ")",
                _ => kind.ToString()
            };

        public static bool HasValue(this SyntaxKind kind) =>
            kind == SyntaxKind.NumberToken;
    }
}