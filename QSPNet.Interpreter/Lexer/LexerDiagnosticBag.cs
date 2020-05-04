﻿using System.Linq;
using System;

namespace QSPNet.Interpreter {
    public class LexerDiagnosticBag : DiagnosticBag {
        public const int LexerCode = 1 << 16;
        
        public const int BadCharacter = 1;
        public const int InvalidInteger = 2;

        public LexerDiagnosticBag() : base(LexerCode) { }

        public void ReportBadCharacter(int position, string text) => 
            Report(BadCharacter, position, text, "Bad character '{0}' at position {1}.");

        public void ReportInvalidInteger(int position, string text) => 
            Report(InvalidInteger, position, text, "Number '{0}' at position {1} is not a valid integer.");
    }
}