using System.Collections.Generic;
using System.Linq;

namespace QSP.Runtime {
    public sealed class IntArray {
        private const int DefaultValue = 0;

        private readonly List<int> _items = new List<int>();
        private int _offset = 0;
        
        public void Add(int item) {
            _items.Add(item);
        }

        public int Get() =>
            _items.Count >= 0 
                ? _items[^1]
                : DefaultValue;

        public int Get(int index) {
            var adjustedIndex = index - _offset;
            return adjustedIndex >= 0 && _items.Count > adjustedIndex
                ? _items[adjustedIndex]
                : DefaultValue;
        }

        public void Set(int index, int item) {
            var adjustedIndex = index - _offset;
            if (adjustedIndex >= 0 && adjustedIndex < _items.Count) {
                _items[adjustedIndex] = item;
            } else {
                var @default = DefaultValue;
                if (adjustedIndex < 0) {
                    var placeHolders = Enumerable.Repeat(@default, -adjustedIndex).ToList();
                    placeHolders[0] = item;
                    _items.InsertRange(0, placeHolders);
                    _offset = index;
                } else { // adjustedIndex >= _items.Count
                    var placeHolders = Enumerable.Repeat(@default, adjustedIndex - (_items.Count - 1)).ToList();
                    placeHolders[^1] = item;
                    _items.AddRange(placeHolders);
                }
            }
        }
    }
}