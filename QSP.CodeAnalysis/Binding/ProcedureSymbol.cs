using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class ProcedureSymbol {
        private ProcedureSymbol(string name, ImmutableArray<BoundType> argumentsTypes, bool canHaveModifier = false) {
            Name            = name;
            ArgumentsTypes  = argumentsTypes;
            CanHaveModifier = canHaveModifier;
        }
        
        public string Name { get; }
        public ImmutableArray<BoundType> ArgumentsTypes { get; }
        public bool CanHaveModifier { get; }

        public static ProcedureSymbol PrintLine { get; } = new ProcedureSymbol(nameof(PrintLine), ImmutableArray.Create(BoundType.String), true);

        public static ProcedureSymbol? Get(SyntaxTokenKind functionKind) =>
            functionKind switch {
                SyntaxTokenKind.PrintLineProc => PrintLine,
                _                             => null
            };
    }
}