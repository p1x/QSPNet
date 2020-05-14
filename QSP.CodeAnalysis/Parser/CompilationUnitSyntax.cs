using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class CompilationUnitSyntax : SyntaxNode {
        public CompilationUnitSyntax(ImmutableArray<StatementSyntax> statements, SyntaxToken endOfFileToken) {
            if (statements.IsDefault)
                throw new ArgumentNullException(nameof(statements));
            Statements = statements;
            EndOfFileToken = endOfFileToken ?? throw new ArgumentNullException(nameof(endOfFileToken));
        }
        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
        
        public ImmutableArray<StatementSyntax> Statements { get; }
        
        public SyntaxToken EndOfFileToken { get; }
        
        public override IEnumerable<object> GetChildren() {
            foreach (var statement in Statements)
                yield return statement;

            yield return EndOfFileToken;
        }
    }
}