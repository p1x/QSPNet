using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class BoundProcedureStatement : BoundStatement {
        public ProcedureSymbol Procedure { get; }
        
        public bool WithModifier { get; }
        
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundProcedureStatement(ProcedureSymbol procedureSymbol, bool withModifier, ImmutableArray<BoundExpression> arguments) : base(BoundNodeKind.ProcedureStatement) {
            Procedure    = procedureSymbol;
            WithModifier = withModifier;
            Arguments    = arguments;
        }
    }
}