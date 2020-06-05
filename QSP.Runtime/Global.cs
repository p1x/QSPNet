using System;

namespace QSP.Runtime {
    public static class Global {
        public static void PrintLineMain(int value) => PrintLineMain(value.ToString());
        public static void PrintLineMain(string line) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        public static void PrintLine(int value) => PrintLine(value.ToString());
        public static void PrintLine(string line) {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        public static string Input(string message) {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
    }
}