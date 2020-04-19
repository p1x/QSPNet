using System;

namespace QSPNet.Interpreter {
    public static class ColoredConsole {
        public static void WriteLine(string text) {
            foreach (var part in text.Split('#')) {
                if (part.ToUpper() == "RESET")
                    Console.ResetColor();
                else if (Enum.TryParse<ConsoleColor>(part, true, out var color))
                    Console.ForegroundColor = color;
                else
                    Console.Write(part);
            }
            Console.WriteLine();
        }
    }
}