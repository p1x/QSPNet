using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class BoundProcedureStatement : BoundStatement {
        public ProcedureSymbol Procedure { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }

        public BoundProcedureStatement(ProcedureSymbol procedureSymbol, ImmutableArray<BoundExpression> arguments) : base(BoundNodeKind.ProcedureStatement) {
            Procedure = procedureSymbol;
            Arguments       = arguments;
        }
    }
}