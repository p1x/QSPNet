using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class ProcedureSymbol {
        private ProcedureSymbol(string name, ImmutableArray<BoundType> argumentsTypes) {
            Name           = name;
            ArgumentsTypes = argumentsTypes;
        }
        
        public string Name { get; }
        public ImmutableArray<BoundType> ArgumentsTypes { get; }
        public static ProcedureSymbol PrintLine { get; } = new ProcedureSymbol(nameof(PrintLine), ImmutableArray.Create(BoundType.String));

        public static ProcedureSymbol? Get(SyntaxTokenKind functionKind) =>
            functionKind switch {
                SyntaxTokenKind.PrintLineProc => PrintLine,
                _                                 => null
            };
    }
}