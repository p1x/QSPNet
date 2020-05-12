using System;
using System.Collections.Generic;
using System.Text;

namespace QSPNet.Interpreter {
    static class Program {
        private static ReplOptions _replOptions = ReplOptions.None;

        private static void Main(string[] args) {
            var text = new StringBuilder();
            var variables = new Dictionary<string, object>();
            while (true) {
                if(text.Length == 0)
                    Console.Write("> ");
                else
                    Console.Write("· ");

                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.StartsWith('/')) {
                    switch (line.ToUpperInvariant()) {
                        case "/Q":
                        case "/EXIT":
                            return;
                        case "/CLS":
                            Console.Clear();
                            break;
                        case "/L":
                        case "/LEX":
                        case "/SHOWLEX":
                            _replOptions = SwitchFlag(ReplOptions.PrintLexedTokens);
                            var printLexString = _replOptions.HasFlag(ReplOptions.PrintLexedTokens)
                                ? $"#{ConsoleColor.Green}#enabled#RESET#"
                                : $"#{ConsoleColor.Red}#disabled#RESET#";
                            ColoredConsole.WriteLine($"Printing lexed tokens {printLexString}.");
                            break;
                        case "/S":
                        case "/SYNTAX":
                        case "/SHOWSYNTAX":
                            _replOptions = SwitchFlag(ReplOptions.PrintSyntaxTokens);
                            var printSyntaxString = _replOptions.HasFlag(ReplOptions.PrintSyntaxTokens)
                                ? $"#{ConsoleColor.Green}#enabled#RESET#"
                                : $"#{ConsoleColor.Red}#disabled#RESET#";
                            ColoredConsole.WriteLine($"Printing syntax tree {printSyntaxString}.");
                            break;
                        case "/R":
                        case "/RUN":
                            Process(text.ToString(), variables);
                            variables.Clear();
                            break;
                        case "/RS":
                        case "/RESET":
                            variables.Clear();
                            text.Clear();
                            break;
                        default:
                            Console.Error.WriteLine($"Unknown command: {line}.");
                            break;
                    }    
                } else {
                    text.AppendLine(line);
                }
            }
        }

        private static ReplOptions SwitchFlag(ReplOptions flag) => (_replOptions & flag) != 0 ? _replOptions & ~flag : _replOptions ^ flag;

        private static void Process(string line, Dictionary<string, object> variables) {
            if(_replOptions.HasFlag(ReplOptions.PrintLexedTokens)) {
                var lexer = new Lexer(line);
                var tokens = lexer.Lex();
                Console.WriteLine("=== Lexed Tokens ===");
                foreach (var token in tokens)
                    Console.WriteLine(token.ToString());
                Console.WriteLine();
            }
            
            var parser = new Parser(line);
            var (syntaxTree, diagnostics) = parser.Parse();

            if (_replOptions.HasFlag(ReplOptions.PrintSyntaxTokens)) {
                static void print(object o, int i) {
                    if (o is IEnumerable<StatementSyntax> ss) {
                        foreach (var s in ss) {
                            print(s, i + 1);
                        }
                        return;
                    }
                    
                    if (o is SyntaxToken t && t.IsManufactured)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(new string(' ', i * 4));
                    Console.WriteLine(o);
                    Console.ResetColor();
                    
                    if (o is SyntaxNode n) {
                        foreach (var c in n.GetChildren()) {
                            print(c, i + 1);
                        }
                    }
                }
                Console.WriteLine("=== Syntax Tree ===");
                print(syntaxTree.Statements, 0);
                Console.WriteLine();
            }
            
            foreach (var d in diagnostics)
                PrintDiagnostics(d);

            if (diagnostics.Count == 0) {
                foreach (var statement in syntaxTree.Statements) {
                    var evaluator = new Evaluator(statement, variables);
                    var result = evaluator.Evaluate();

                    switch (result) {
                        case VariableChangeResult v:
                            variables[v.VariableName] = v.Value;
                            break;
                        default:
                            Console.WriteLine(result.Value);
                            break;
                    }
                }
            }
        }

        private static void PrintDiagnostics(Diagnostics diagnostics) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.Write($"[{diagnostics.ErrorCode:x4}] ");
            Console.Error.WriteLine(diagnostics.GetFormattedMessage());
            Console.ResetColor();
        }
    }
}