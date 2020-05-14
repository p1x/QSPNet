using System;

namespace QSP.CodeAnalysis {
    public class SyntaxToken {
        public SyntaxToken(SyntaxTokenKind kind, int position, string text, object value) : this(kind, position) {
            if (!kind.HasValue())
                throw new AggregateException($"Token with kind '{kind}' can't contain value.");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Token text cannot be null or empty.", nameof(text));

            Value = value ?? throw new ArgumentNullException(nameof(value));
            Text = text;
        }
        
        public SyntaxToken(SyntaxTokenKind kind, int position, string text) : this(kind, position) {
            if (kind.HasValue())
                throw new AggregateException($"Token with kind '{kind}' should contain value.");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Token text cannot be null or empty.", nameof(text));
            Text = text;
        }

        private SyntaxToken(SyntaxTokenKind kind, int position) {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position));
            
            Kind = kind;
            Position = position;
            Text = string.Empty;
        }

        public static SyntaxToken Manufacture(SyntaxTokenKind kind, int position) => new SyntaxToken(kind, position);
        
        public bool IsManufactured => string.IsNullOrEmpty(Text);
        
        public override string ToString() => $"{Kind}:{Position} '{Text}'";

        public SyntaxTokenKind Kind { get; }
        public int Position { get; }
        public string Text { get; }

        public object? Value { get; } = null;
    }
}