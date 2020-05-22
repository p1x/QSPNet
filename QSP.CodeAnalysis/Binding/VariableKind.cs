using System;

namespace QSP.CodeAnalysis {
    [Flags]
    public enum VariableKind {
        None       = 0b00,
        Array      = 0b01,
        Dictionary = 0b10,
        Mixed      = Array | Dictionary
    }
}