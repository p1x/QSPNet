using System.Collections;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class DiagnosticBag : IReadOnlyList<Diagnostics> {
        private readonly int _baseCode;
        
        private readonly List<Diagnostics> _items = new List<Diagnostics>();
        protected DiagnosticBag(int baseCode) => _baseCode = baseCode;

        protected void Report(int errorCode, int position, string text, string message) => 
            _items.Add(new Diagnostics(errorCode ^ _baseCode, position, message, text));
        
        public IEnumerator<Diagnostics> GetEnumerator() => _items.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _items.Count;

        public Diagnostics this[int index] => _items[index];

        public DiagnosticBag With(DiagnosticBag other) {
            var merged = new DiagnosticBag(0);
            merged._items.AddRange(this._items);
            merged._items.AddRange(other._items);
            return merged;
        }
    }
}