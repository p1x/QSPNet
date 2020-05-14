using System;

namespace QSP.CodeAnalysis {
    public readonly struct Precedence {
        public Precedence(int value) {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Precedence value should be non-negative.");
            
            Value = value;
        }
        
        public int Value { get; }
        
        public static implicit operator Precedence(int v) => new Precedence(v); 
        public static implicit operator int(Precedence p) => p.Value;
    }
}