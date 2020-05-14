using System;

namespace QSP.CodeAnalysis {
    public class SyntaxTree {
        public SyntaxTree(string text, CompilationUnitSyntax root, DiagnosticBag diagnostics) {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));;
        }

        public string Text { get; }
        
        public DiagnosticBag Diagnostics { get; }
        
        public CompilationUnitSyntax Root { get; }
    }
}