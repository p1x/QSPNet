using System;
using System.Collections.Generic;
using System.IO;

namespace QSPNet.Interpreter {
    public class Evaluator {
        private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();
        private readonly CompilationUnitSyntax _compilationUnit;
        private readonly TextReader _reader;
        private readonly TextWriter _writer;
        
        public Evaluator(CompilationUnitSyntax compilationUnit, TextReader reader, TextWriter writer) {
            _compilationUnit = compilationUnit;
            _reader = reader;
            _writer = writer;
        }

        public void Evaluate() {
            foreach (var statement in _compilationUnit.Statements) {
                var r = EvaluateStatement(statement);
                if (r is VariableChangeResult v) {
                    _variables[v.VariableName] = v.Value;
                } else {
                    _writer.WriteLine(r.Value.ToString());
                }
            }
        }
        
        private EvaluationResult EvaluateStatement(StatementSyntax statement) =>
            statement switch {
                ExpressionStatementSyntax e => EvaluateExpressionStatement(e),
                AssignmentStatementSyntax a => EvaluateAssignmentStatement(a),
                _ => throw new ArgumentOutOfRangeException()
            };

        private EvaluationResult EvaluateExpressionStatement(ExpressionStatementSyntax syntax) {
            var value = EvaluateExpression(syntax.Expression);
            return new EvaluationResult(value);
        }

        private EvaluationResult EvaluateAssignmentStatement(AssignmentStatementSyntax syntax) {
            var variableName = syntax.Identifier.Text;
            var value = EvaluateExpression(syntax.Expression);
            return new VariableChangeResult(variableName, value);
        }

        private object EvaluateExpression(ExpressionSyntax expression) =>
            expression switch {
                LiteralExpressionSyntax       l => EvaluateLiteralExpression(l),
                UnaryExpressionSyntax         u => EvaluateUnaryExpression(u),
                BinaryExpressionSyntax        b => EvaluateBinaryExpression(b),
                NameExpressionSyntax          n => EvaluateNameExpression(n),
                ParenthesisedExpressionSyntax p => EvaluateParenthesisedExpression(p),
                _ => string.Empty
            };

        private static object EvaluateLiteralExpression(LiteralExpressionSyntax n) =>
            n.Token.Value switch {
                int _ => n.Token.Value,
                string _ => n.Token.Value,
                _ => throw new NotSupportedException("Literals other then numbers and strings are not supported.")
            };

        private object EvaluateUnaryExpression(UnaryExpressionSyntax u) =>
            u.Operator.Kind switch {
                SyntaxTokenKind.Plus => EvaluateExpression(u.Operand),
                SyntaxTokenKind.Minus => -(int)EvaluateExpression(u.Operand),
                SyntaxTokenKind.Input => EvaluateInput(EvaluateExpression(u.Operand)),
                _ => string.Empty
            };

        private object EvaluateBinaryExpression(BinaryExpressionSyntax b) {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            if (b.Operator.Kind == SyntaxTokenKind.Ampersand)
                return string.Concat(left, right);
            
            if ((left  is int leftInt  || int.TryParse(left.ToString(),  out leftInt)) &&
                (right is int rightInt || int.TryParse(right.ToString(), out rightInt))) {

                return b.Operator.Kind switch {
                    SyntaxTokenKind.Plus => (int)leftInt + (int)rightInt,
                    SyntaxTokenKind.Minus => (int)leftInt - (int)rightInt,
                    SyntaxTokenKind.Slash => (int)leftInt / (int)rightInt,
                    SyntaxTokenKind.Star => (int)leftInt * (int)rightInt,
                    SyntaxTokenKind.Mod => (int)leftInt % (int)rightInt,
                    _ => string.Empty
                };
            }
            
            if (left is string || right is string && b.Operator.Kind == SyntaxTokenKind.Plus)
                return string.Concat(left, right);

            throw new NotSupportedException();
        }

        private object EvaluateNameExpression(NameExpressionSyntax n) {
            return _variables[n.Identifier.Text];
        }

        private object EvaluateParenthesisedExpression(ParenthesisedExpressionSyntax p) {
            return EvaluateExpression(p.Expression);
        }

        private string EvaluateInput(object message) {
            _writer.WriteLine(message);
            _writer.Write("> ");
            return _reader.ReadLine() ?? string.Empty;
        }
    }
}