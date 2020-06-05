using System;

namespace QSP.Runtime {
    public static class Global {
        public static void PrintLineMain(object value) => PrintLineMain(value.ToString());
        public static void PrintLineMain(int value) => PrintLineMain(value.ToString());
        public static void PrintLineMain(string line) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        public static void PrintLine(object value) => PrintLine(value.ToString());
        public static void PrintLine(int value) => PrintLine(value.ToString());
        public static void PrintLine(string line) {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        public static string Input(object message) => Input(message.ToString());
        public static string Input(int message) => Input(message.ToString());
        public static string Input(string message) {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static object DynamicAdd(int a, string b) {
            if (int.TryParse(b, out var bInt))
                return a + bInt;
            return string.Concat(a.ToString(), b);
        }

        public static object DynamicAdd(string a, int b) {
            if (int.TryParse(a, out var aInt))
                return aInt + b;
            return string.Concat(a, b.ToString());
        }
        
        public static object DynamicAdd(int a, object b) =>
            b switch {
                string y => DynamicAdd(a, y),
                int y    => a + y,
                _        => throw new ArgumentException("Unsupported runtime type", nameof(b))
            };

        public static object DynamicAdd(object a, int b) =>
            a switch {
                string x => DynamicAdd(x, b),
                int x    => x + b,
                _        => throw new ArgumentException("Unsupported runtime type", nameof(a))
            };

        public static object DynamicAdd(string a, object b) =>
            b switch {
                string y => string.Concat(a, y),
                int y    => DynamicAdd(a, y),
                _        => throw new ArgumentException("Unsupported runtime type", nameof(b))
            };

        public static object DynamicAdd(object a, string b) =>
            a switch {
                string x => string.Concat(x, b),
                int x    => DynamicAdd(x, b),
                _           => throw new ArgumentException("Unsupported runtime type", nameof(a))
            };

        public static object DynamicAdd(object a, object b) =>
            a switch {
                string x => DynamicAdd(x, b),
                int x    => DynamicAdd(x, b),
                _        => throw new ArgumentException("Unsupported runtime type", nameof(a))
            };
    }
}