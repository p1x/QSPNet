using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class FunctionSymbol {
        private FunctionSymbol(string name, ImmutableArray<BoundType> argumentsTypes, BoundType type) {
            Name           = name;
            ArgumentsTypes = argumentsTypes;
            Type           = type;
        }
        
        public string Name { get; }
        public ImmutableArray<BoundType> ArgumentsTypes { get; }
        public BoundType Type { get; }
        public static FunctionSymbol Input { get; } = new FunctionSymbol("Input", ImmutableArray.Create(BoundType.String), BoundType.String);

        public static FunctionSymbol? Get(SyntaxTokenKind functionKind) =>
            functionKind switch {
                SyntaxTokenKind.InputFunc => Input,
                _                         => null
            };
    }
}