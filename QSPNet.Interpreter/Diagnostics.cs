using System;

namespace QSPNet.Interpreter {
    public class Diagnostics {
        public Diagnostics(int errorCode, int position, string message, string text) {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position));
            
            ErrorCode = errorCode;
            Position = position;
            Message = message;
            Text = text;
        }

        public string GetFormattedMessage() => string.Format(Message, Text, Position.ToString());
        
        public int ErrorCode { get; }
        public int Position { get; }
        public string Message { get; }
        public string Text { get; }
    }
}