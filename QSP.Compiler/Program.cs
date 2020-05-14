using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace QSP.Compiler {
    internal static class Program {
        private static int Main(string[] args) {
            var inputPaths = new Argument<string> {
                Arity = ArgumentArity.OneOrMore,
                Name = nameof(Options.Sources),
                Description = "Source files to process.",
            };
            var outputPaths = new Option(new[] { "--output", "-o" }) {
                Name = nameof(Options.OutputPath),
                Description = "The output path of the assembly to create.",
                Argument = new Argument<string> {
                    Arity = ArgumentArity.ExactlyOne
                },
                Required = true,
            };
            var moduleName = new Option(new[] { "--module-name", "-m" }) {
                Name = nameof(Options.ModuleName),
                Description = "The name of the module.",
                Argument = new Argument<string> {
                    Arity = ArgumentArity.ExactlyOne
                }
            };
            var references = new Option(new[] { "--reference", "-r" }) {
                Name = nameof(Options.References),
                Description = "The path of an assembly to reference.",
                Argument = new Argument<string> {
                    Arity = ArgumentArity.OneOrMore
                }
            };
            var rootCommand = new RootCommand("QSP Compiler for .NET Core.") {
                inputPaths,
                outputPaths,
                moduleName,
                references,
            };

            rootCommand.TreatUnmatchedTokensAsErrors = true;
            rootCommand.Handler = CommandHandler.Create<Options>(o => Run(
                o.Sources ?? throw new InvalidOperationException(),
                o.OutputPath ?? throw new InvalidOperationException(),
                o.ModuleName,
                o.References ?? Array.Empty<string>()));

            return rootCommand.InvokeAsync(args).Result;
        }

        public static int Run(string[] sources, string outputPath, string? moduleName, string[] references) {
            return 0;
        }
    }
}