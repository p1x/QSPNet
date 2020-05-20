using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace QSP.CodeAnalysis {
    public class Parser : ParserBase {
        public Parser(string text) : base(text) { }

        protected override CompilationUnitSyntax ParseCore() {
            var statements = new List<StatementSyntax>();
            while (Current.Kind != SyntaxTokenKind.EndOfFile) {
                if (Current.Kind == SyntaxTokenKind.EndOfLine) {
                    Next();
                    continue;
                }
                
                var startToken = Current;
                
                statements.Add(ParseStatement());

                // ParseStatement should always consume at least one token,
                // but if didn't we consume one here to prevent infinite loop
                if (startToken == Current) {
                    Debug.Fail("startToken == Current");
                    Next();
                }
            }

            var eof = Match(SyntaxTokenKind.EndOfFile);
            return new CompilationUnitSyntax(statements.ToImmutableArray(), eof);
        }

        private StatementSyntax ParseStatement() {
            if (Current.Kind == SyntaxTokenKind.Identifier && Lookahead.Kind == SyntaxTokenKind.Equals)
                return ParseAssignmentStatement();
            if (Current.Kind == SyntaxTokenKind.PrintLineProc)
                return ParseProcedureStatement(SyntaxTokenKind.PrintLineProc);
            return ParseExpressionStatement();
        }

        private StatementSyntax ParseAssignmentStatement() {
            var identifierToken = Match(SyntaxTokenKind.Identifier);
            var equalsToken     = Match(SyntaxTokenKind.Equals);
            var expression      = ParseExpression();
            var endOfLineToken  = MatchEndOfStatement();
            return new AssignmentStatementSyntax(identifierToken, equalsToken, expression, endOfLineToken);
        }

        private StatementSyntax ParseProcedureStatement(SyntaxTokenKind kind) {
            var modifier = TryNext(SyntaxTokenKind.Star);
            var keyword = Match(kind);
            var openParenthesis = TryNext(SyntaxTokenKind.OpenParenthesis);
            var arguments = ParseArguments(openParenthesis != null, 0);
            var closeParenthesis = openParenthesis != null ? Match(SyntaxTokenKind.CloseParenthesis) : null;
            var endOfLineToken = MatchEndOfStatement();
            return new ProcedureStatementSyntax(modifier, keyword, openParenthesis, arguments, closeParenthesis, endOfLineToken);
        }

        private StatementSyntax ParseExpressionStatement() {
            var expression     = ParseExpression();
            var endOfLineToken = MatchEndOfStatement();
            return new ExpressionStatementSyntax(expression, endOfLineToken);
        }

        private ExpressionSyntax ParseExpression(bool parseConcat = false, Precedence parentPrecedence = default) {
            var left = ParseExpressionLeft(parseConcat, parentPrecedence);

            // if parseConcat - we parse '&' as string concatenation token
            // else - we parse '&' as statement chaining
            Precedence precedence;
            while ((precedence = Current.Kind.GetBinaryPrecedence()) > parentPrecedence &&
                   (parseConcat || Current.Kind != SyntaxTokenKind.Ampersand)
            ) {
                var operatorToken = Next();
                var right = ParseExpression(parseConcat, precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParseExpressionLeft(bool parseConcat, Precedence parentPrecedence) {
            // if current is unary operator (precedence > 0) and its precedence > parentPrecedence
            var unaryPrecedence = Current.Kind.GetUnaryPrecedence();
            if (unaryPrecedence > parentPrecedence) {
                var operatorToken = Next();
                var operand = ParseExpression(parseConcat, unaryPrecedence);
                return new UnaryExpressionSyntax(operatorToken, operand);
            }

            return ParsePrimaryExpression();
        }

        private ExpressionSyntax ParsePrimaryExpression() =>
            Current.Kind switch {
                SyntaxTokenKind.OpenParenthesis => ParseParenthesisedExpression(),
                SyntaxTokenKind.Number          => new LiteralExpressionSyntax(Match(SyntaxTokenKind.Number)),
                SyntaxTokenKind.String          => new LiteralExpressionSyntax(Match(SyntaxTokenKind.String)),
                SyntaxTokenKind.InputFunc       => ParseFunctionExpression(SyntaxTokenKind.InputFunc),
                _                               => new NameExpressionSyntax(Match(SyntaxTokenKind.Identifier))
            };

        private FunctionExpressionSyntax ParseFunctionExpression(SyntaxTokenKind funcKind) {
            var hasSingleArgument = funcKind.FunctionHasSingleArgument();
            var keyword = Match(funcKind);

            // If function has single argument, it can be called without parenthesis.
            var openParenthesis = hasSingleArgument ? TryNext(SyntaxTokenKind.OpenParenthesis) : Match(SyntaxTokenKind.OpenParenthesis);
            var isParenthesised = openParenthesis != null;

            // And if it called without parenthesis, it should be parsed with precedence between binary and unary.
            var precedence = isParenthesised ? 0 : 1 << 16; // Just below unary precedence
            var arguments = ParseArguments(isParenthesised, precedence);
                
            var closeParenthesis = isParenthesised ? Match(SyntaxTokenKind.CloseParenthesis) : null;

            return new FunctionExpressionSyntax(keyword, openParenthesis, arguments, closeParenthesis);
        }

        private SeparatedListSyntax<ExpressionSyntax> ParseArguments(bool parenthesised, Precedence parentPrecedence) {
            ImmutableArray<ExpressionSyntax> arguments;
            ImmutableArray<SyntaxToken>      separators;

            var argumentsBuilder  = ImmutableArray.CreateBuilder<ExpressionSyntax>();
            var separatorsBuilder = ImmutableArray.CreateBuilder<SyntaxToken>();
            do {
                var argument = ParseExpression(parenthesised, parentPrecedence);
                argumentsBuilder.Add(argument);
                if (Current.Kind != SyntaxTokenKind.Comma)
                    break;
                var separator = Next();
                separatorsBuilder.Add(separator);
            } while (true);

            arguments  = argumentsBuilder.ToImmutable();
            separators = separatorsBuilder.ToImmutable();
            return new SeparatedListSyntax<ExpressionSyntax>(arguments, separators);
        }

        private ExpressionSyntax ParseParenthesisedExpression() {
            var open = Match(SyntaxTokenKind.OpenParenthesis);
            var expression = ParseExpression(true);
            var close = Match(SyntaxTokenKind.CloseParenthesis);
            return new ParenthesisedExpressionSyntax(open, expression, close);
        }
    }
}