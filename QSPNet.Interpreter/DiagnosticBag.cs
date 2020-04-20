using System.Collections;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public abstract class DiagnosticBag : IReadOnlyList<Diagnostics> {
        private readonly List<Diagnostics> _items = new List<Diagnostics>();

        protected virtual void Report(int errorCode, int position, string text, string message) => _items.Add(new Diagnostics(errorCode, position, message, text));
        
        public IEnumerator<Diagnostics> GetEnumerator() => _items.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _items.Count;

        public Diagnostics this[int index] => _items[index];
    }
}