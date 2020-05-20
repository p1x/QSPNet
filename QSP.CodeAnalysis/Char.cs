namespace QSP.CodeAnalysis {
    public static class Char {
        public const char Null = (char)0;
        public static readonly string NullString = Null.ToString();
        public static bool IsNull(char c) => c == (char)0;
        
        public static bool IsIdentifier(char c) => char.IsLetter(c) || c == '$' || c == '#' || c == '_';
    }
}