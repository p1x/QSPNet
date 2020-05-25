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
            return a.ToString() + b;
        }

        public static object DynamicAdd(string a, int b) {
            if (int.TryParse(a, out var aInt))
                return aInt + b;
            return a + b.ToString();
        }
        
        public static object DynamicAdd(int a, object b) {
            if (b is string bStr)
                return DynamicAdd(a, bStr);
            if (b is int bInt)
                return a + bInt;
            
            throw new ArgumentException("Unsupported runtime type", nameof(b));
        }
        
        public static object DynamicAdd(object a, int b) {
            if (a is string aStr)
                return DynamicAdd(aStr, b);
            if (a is int aInt)
                return aInt + b;
            
            throw new ArgumentException("Unsupported runtime type", nameof(a));
        }
        
        public static object DynamicAdd(string a, object b) {
            if (b is int bInt)
                return DynamicAdd(a, bInt);
            if (b is string bStr)
                return a + bStr;
            
            throw new ArgumentException("Unsupported runtime type", nameof(b));
        }

        public static object DynamicAdd(object a, string b) {
            if (a is int aInt)
                return DynamicAdd(aInt, b);
            if (a is string aStr)
                return aStr + b;

            throw new ArgumentException("Unsupported runtime type", nameof(a));
        }

        public static object DynamicAdd(object a, object b) {
            if (a is string aStr)
                return DynamicAdd(aStr, b);
            if (a is int aInt)
                return DynamicAdd(aInt, b);

            throw new ArgumentException("Unsupported runtime type", nameof(a));
        }
    }
}