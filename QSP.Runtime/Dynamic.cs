using System.Runtime.InteropServices;

namespace QSP.Runtime {
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct Dynamic {
        [FieldOffset(0)]
        private readonly bool _isInt;
        
        [FieldOffset(4)]
        private readonly int _intValue;

        [FieldOffset(4)]
        private readonly string _stringValue;

        public Dynamic(int value) {
            _stringValue = default;

            _isInt = true;
            _intValue = value;
        }

        public Dynamic(string value) {
            _intValue = default;

            _isInt = true;
            _stringValue = value;
        }

        public Dynamic Add(Dynamic a, Dynamic b) {
            if(!a._isInt && !b._isInt)
                return new Dynamic(string.Concat(a._stringValue, b._stringValue));
            
            if (!a._isInt && int.TryParse(a._stringValue, out var aParsed))
                a = new Dynamic(aParsed);
            if (!b._isInt && int.TryParse(b._stringValue, out var bParsed))
                b = new Dynamic(bParsed);
            
            if(a._isInt && b._isInt)
                return new Dynamic(a._intValue + b._intValue);

            return new Dynamic(string.Concat(a.ToString(), b.ToString()));
        }

        public override string ToString() => _isInt ? _intValue.ToString() : _stringValue;
    }
}