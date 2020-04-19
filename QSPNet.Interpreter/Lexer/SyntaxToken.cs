using System;

namespace QSPNet.Interpreter {
    public class SyntaxToken {
        public SyntaxToken(SyntaxKind kind, int position, string text) : this(kind, position) {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            Text = text;
        }

        private SyntaxToken(SyntaxKind kind, int position) {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position));
            
            Kind = kind;
            Position = position;
            Text = string.Empty;
        }

        public static SyntaxToken Manufacture(SyntaxKind kind, int position) => new SyntaxToken(kind, position);
        
        public bool IsManufactured => string.IsNullOrEmpty(Text);
        
        public override string ToString() => $"{Kind}:{Position} '{Text}'";

        public SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
    }
}