namespace QSP.CodeAnalysis {
    public static partial class SyntaxFacts {
        public static bool HasValue(this SyntaxKind kind) => HasValue(kind.AsToken());
        public static bool HasValue(this SyntaxTokenKind kind) => 
            kind == SyntaxTokenKind.Number ||
            kind == SyntaxTokenKind.String;

        // TODO move to generated SyntaxFacts
        public static bool IsFunc(this SyntaxTokenKind kind) =>
            kind == SyntaxTokenKind.InputFunc;
    }
}