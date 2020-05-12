using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
        public class ParenthesisedExpressionSyntax : ExpressionSyntax {
            public ParenthesisedExpressionSyntax(SyntaxToken openParenthesis, ExpressionSyntax expression, SyntaxToken closeParenthesis) {
                OpenParenthesis = openParenthesis ?? throw new ArgumentNullException(nameof(openParenthesis)); 
                Expression = expression ?? throw new ArgumentNullException(nameof(expression)); 
                CloseParenthesis = closeParenthesis ?? throw new ArgumentNullException(nameof(closeParenthesis)); 
            }

            public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Parenthesised;

            public SyntaxToken OpenParenthesis { get; }
            public ExpressionSyntax Expression { get; }
            public SyntaxToken CloseParenthesis { get; }

            public override IEnumerable<object> GetChildren() {
                yield return OpenParenthesis;
                yield return Expression;
                yield return CloseParenthesis;
            }   
        }
        public class BinaryExpressionSyntax : ExpressionSyntax {
            public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken @operator, ExpressionSyntax right) {
                Left = left ?? throw new ArgumentNullException(nameof(left)); 
                Operator = @operator ?? throw new ArgumentNullException(nameof(@operator)); 
                Right = right ?? throw new ArgumentNullException(nameof(right)); 
            }

            public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Binary;

            public ExpressionSyntax Left { get; }
            public SyntaxToken Operator { get; }
            public ExpressionSyntax Right { get; }

            public override IEnumerable<object> GetChildren() {
                yield return Left;
                yield return Operator;
                yield return Right;
            }   
        }
        public class UnaryExpressionSyntax : ExpressionSyntax {
            public UnaryExpressionSyntax(SyntaxToken @operator, ExpressionSyntax operand) {
                Operator = @operator ?? throw new ArgumentNullException(nameof(@operator)); 
                Operand = operand ?? throw new ArgumentNullException(nameof(operand)); 
            }

            public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Unary;

            public SyntaxToken Operator { get; }
            public ExpressionSyntax Operand { get; }

            public override IEnumerable<object> GetChildren() {
                yield return Operator;
                yield return Operand;
            }   
        }
        public class NameExpressionSyntax : ExpressionSyntax {
            public NameExpressionSyntax(SyntaxToken identifier) {
                Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier)); 
            }

            public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Name;

            public SyntaxToken Identifier { get; }

            public override IEnumerable<object> GetChildren() {
                yield return Identifier;
            }   
        }
        public class LiteralExpressionSyntax : ExpressionSyntax {
            public LiteralExpressionSyntax(SyntaxToken token) {
                Token = token ?? throw new ArgumentNullException(nameof(token)); 
            }

            public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Literal;

            public SyntaxToken Token { get; }

            public override IEnumerable<object> GetChildren() {
                yield return Token;
            }   
        }
        public class ExpressionStatementSyntax : StatementSyntax {
            public ExpressionStatementSyntax(ExpressionSyntax expression, SyntaxToken endToken) : base(endToken) {
                Expression = expression ?? throw new ArgumentNullException(nameof(expression)); 
            }

            public override SyntaxStatementKind StatementKind => SyntaxStatementKind.Expression;

            public ExpressionSyntax Expression { get; }

            public override IEnumerable<object> GetChildren() {
                yield return Expression;
                foreach (var child in base.GetChildren())
                    yield return child;
            }   
        }
        public class AssignmentStatementSyntax : StatementSyntax {
            public AssignmentStatementSyntax(SyntaxToken identifier, SyntaxToken equals, ExpressionSyntax expression, SyntaxToken endToken) : base(endToken) {
                Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier)); 
                Equals = equals ?? throw new ArgumentNullException(nameof(equals)); 
                Expression = expression ?? throw new ArgumentNullException(nameof(expression)); 
            }

            public override SyntaxStatementKind StatementKind => SyntaxStatementKind.Assignment;

            public SyntaxToken Identifier { get; }
            public SyntaxToken Equals { get; }
            public ExpressionSyntax Expression { get; }

            public override IEnumerable<object> GetChildren() {
                yield return Identifier;
                yield return Equals;
                yield return Expression;
                foreach (var child in base.GetChildren())
                    yield return child;
            }   
        }
}

