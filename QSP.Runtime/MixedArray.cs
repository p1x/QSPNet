using System.Collections.Generic;
using System.Linq;

namespace QSP.Runtime {
    public class MixedArray<T> {
        private readonly List<T> _values = new List<T>();
        private readonly Dictionary<string, int> _stringPointers = new Dictionary<string, int>();
        
        public void SetValue(int index, T value) {
            if (_values.Count > index) {
                _values[index] = value;
            } else {
                var count = index - _values.Count;
                _values.AddRange(Enumerable.Repeat(default(T), count));
                _values[index] = value;
            }
        }

        public void SetValue(string index, T value) {
            if (_stringPointers.TryGetValue(index, out var actualIndex)) {
                _values[actualIndex] = value;
            } else {
                _values.Add(value);
                _stringPointers[index] = _values.Count - 1;
            }
        }

        public void SetValue(T value) {
            _values.Add(value);
        }

        public T GetValue(int index) {
            return index >= 0 && index < _values.Count 
                ? _values[index] 
                : default;
        }

        public T GetValue(string index) {
            return _stringPointers.TryGetValue(index, out var actualIndex) 
                ? _values[actualIndex]
                : default;
        }

        public T GetValue() => _values.LastOrDefault();

        public void KillVar(int index) {
            if (index < 0 || index >= _values.Count)
                return;

            _values.RemoveAt(index);
            var (key, value) = _stringPointers.FirstOrDefault(x => x.Value == index);
            _stringPointers.Remove(key);
        }
    }
}