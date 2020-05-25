using System;

namespace QSP.CodeAnalysis {
    public class BoundType {
        private BoundType(Type clrType, bool isIntermediate) {
            ClrType = clrType;
            IsIntermediate = isIntermediate;
        }

        public Type ClrType { get; }

        public bool IsIntermediate { get; }

        public static BoundType Undefined { get; } = new BoundType(typeof(object), false);
        public static BoundType Number { get; } = new BoundType(typeof(int),    false);
        public static BoundType String { get; } = new BoundType(typeof(string),  false);
        public static BoundType Any { get; } = new BoundType(typeof(object), true);
    }
}