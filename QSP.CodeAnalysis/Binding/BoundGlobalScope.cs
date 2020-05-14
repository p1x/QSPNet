using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class BoundGlobalScope {
        public BoundGlobalScope(
            ImmutableArray<BoundStatement> statements, 
            ImmutableArray<VariableSymbol> variables,
            DiagnosticBag diagnostics
        ) {
            Statements = statements;
            Variables = variables;
            Diagnostics = diagnostics;
        }

        public ImmutableArray<BoundStatement> Statements { get; }
        
        public ImmutableArray<VariableSymbol> Variables { get; }
        
        public DiagnosticBag Diagnostics { get; }
    }
}