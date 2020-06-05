using System.Collections.Generic;
using System.Linq;

namespace QSP.Runtime {
    public class StringArray {
        private static readonly string DefaultValue = string.Empty;

        private readonly List<string> _items = new List<string>();
        private int _offset = 0;
        
        public void Add(string item) {
            _items.Add(item);
        }

        public string Get() =>
            _items.Count >= 0 
                ? _items[^1]
                : DefaultValue;

        public string Get(int index) {
            var adjustedIndex = index - _offset;
            return adjustedIndex >= 0 && _items.Count > adjustedIndex
                ? _items[adjustedIndex]
                : DefaultValue;
        }

        public void Set(int index, string item) {
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