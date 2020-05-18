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

            return ParseExpressionStatement();
        }

        private StatementSyntax ParseAssignmentStatement() {
            var identifierToken = Match(SyntaxTokenKind.Identifier);
            var equalsToken     = Match(SyntaxTokenKind.Equals);
            var expression      = ParseExpression();
            var endOfLineToken  = MatchEndOfStatement();
            return new AssignmentStatementSyntax(identifierToken, equalsToken, expression, endOfLineToken);
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
                SyntaxTokenKind.Number => new LiteralExpressionSyntax(Match(SyntaxTokenKind.Number)),
                SyntaxTokenKind.String => new LiteralExpressionSyntax(Match(SyntaxTokenKind.String)),
                _ => new NameExpressionSyntax(Match(SyntaxTokenKind.Identifier))
            };

        private ExpressionSyntax ParseParenthesisedExpression() {
            var open = Match(SyntaxTokenKind.OpenParenthesis);
            var expression = ParseExpression(true);
            var close = Match(SyntaxTokenKind.CloseParenthesis);
            return new ParenthesisedExpressionSyntax(open, expression, close);
        }
    }
}