using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class BoundFunctionExpression : BoundExpression {
        public BoundFunctionExpression(FunctionSymbol functionSymbol, ImmutableArray<BoundExpression> arguments) : base(BoundNodeKind.FunctionExpression) {
            Arguments = arguments;
            Function  = functionSymbol;
        }

        public FunctionSymbol Function { get; }
        
        public ImmutableArray<BoundExpression> Arguments { get; }

        public override BoundType Type => Function.Type;
    }
}