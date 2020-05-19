using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class SeparatedListSyntax<T> : IEnumerable<object> where T : SyntaxNode {
        private ImmutableArray<T> _nodes;
        private ImmutableArray<SyntaxToken> _separators;
        
        public SeparatedListSyntax(ImmutableArray<T> nodes, ImmutableArray<SyntaxToken> separators) {
            if (nodes.Length != separators.Length + 1)
                throw new ArgumentException("nodes.Length != separators.Length - 1");
            
            _nodes      = nodes;
            _separators = separators;
        }

        public ImmutableArray<T> Nodes => _nodes;

        public IEnumerator<object> GetEnumerator() {
            if (_nodes.IsEmpty)
                yield break;

            yield return _nodes[0];
            for (var i = 1; i < _nodes.Length; i++) {
                yield return _nodes[i];
                yield return _separators[i - 1];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}