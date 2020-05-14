using System;

namespace QSP.Interpreter {
    [Flags]
    public enum ReplOptions {
        None,
        PrintLexedTokens,
        PrintSyntaxTokens
    }
}