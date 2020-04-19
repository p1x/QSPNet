using System;

namespace QSPNet.Interpreter {
    static class Program {
        private static void Main(string[] args) {
            while (true) {
                Console.Write("> ");

                var line = Console.ReadLine();
                switch (line) {
                    case "/exit":
                        return;
                    case "/cls":
                        Console.Clear();
                        break;
                    default:
                        Process(line);
                        break;
                }
            }
        }

        private static void Process(string line) {
            Console.WriteLine(line);
        }
    }
}