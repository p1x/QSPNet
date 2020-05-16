using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class Compilation {
        private readonly SyntaxTree _syntaxTree;
        
        private Compilation(SyntaxTree syntaxTree) {
            _syntaxTree = syntaxTree;
        }

        public static Compilation Create(SyntaxTree syntaxTree) {
            return new Compilation(syntaxTree);
        } 

        public ImmutableArray<Diagnostics> Emit(string moduleName, string outputPath, string[] references) {
            var scope = Binder.BindScope(_syntaxTree);
            var emitter = Emitter.Create(moduleName, references);
            var diagnosticBag = emitter.Emit(scope, outputPath);
            return diagnosticBag.ToImmutableArray();
        }
    }
}