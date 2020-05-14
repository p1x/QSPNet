using System.Diagnostics.CodeAnalysis;

namespace QSP.Compiler {
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Options {
        public string[]? Sources { get; set; }
        public string? OutputPath { get; set; }
        public string? ModuleName { get; set; }
        public string[]? References { get; set; }
    }
}