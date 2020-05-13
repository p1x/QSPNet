using System;

namespace QSPNet.Interpreter.Binding {
    public class VariableSymbol {
        public VariableSymbol(string name, BoundType type) {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("string.IsNullOrWhiteSpace(name)", nameof(name));
            if (string.Equals(name, name.ToUpperInvariant(), StringComparison.Ordinal))
                throw new ArgumentException("string.Equals(name, name.ToUpperInvariant(), StringComparison.Ordinal)", nameof(name));
            if (type == BoundType.Undefined)
                throw new ArgumentException("type == BoundType.Undefined", nameof(type));
            
            Name = name;
            Type = type;
        }
        
        public string Name { get; }
        public BoundType Type { get; }
    }
}