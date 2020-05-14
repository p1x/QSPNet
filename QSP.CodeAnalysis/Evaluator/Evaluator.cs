using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QSP.CodeAnalysis {
    public class Evaluator {
        private readonly Dictionary<VariableSymbol, object?> _variables;
        private readonly TextReader _reader;
        private readonly TextWriter _writer;
        private readonly BoundGlobalScope _scope;

        public Evaluator(BoundGlobalScope scope, TextReader reader, TextWriter writer) {
            _scope = scope;
            _variables = scope.Variables.ToDictionary(x => x, x => (object?)null);
            
            _reader = reader;
            _writer = writer;
        }

        public void Evaluate() {
            foreach (var statement in _scope.Statements) {
                var r = EvaluateStatement(statement);
                if (r is VariableChangeResult v) {
                    _variables[v.Variable] = v.Value;
                } else {
                    _writer.WriteLine(r.Value.ToString());
                }
            }
        }
        
        private EvaluationResult EvaluateStatement(BoundStatement statement) =>
            statement switch {
                BoundExpressionStatement x => EvaluateExpressionStatement(x),
                BoundAssignmentStatement x => EvaluateAssignmentStatement(x),
                _ => throw new ArgumentOutOfRangeException()
            };

        private EvaluationResult EvaluateExpressionStatement(BoundExpressionStatement s) {
            var value = EvaluateExpression(s.Expression);
            return new EvaluationResult(value);
        }

        private EvaluationResult EvaluateAssignmentStatement(BoundAssignmentStatement s) {
            var value = EvaluateExpression(s.Expression);
            return new VariableChangeResult(s.Variable, value);
        }

        private object EvaluateExpression(BoundExpression expression) =>
            expression switch {
                BoundLiteralExpression       l => EvaluateLiteralExpression(l),
                BoundUnaryExpression         u => EvaluateUnaryExpression(u),
                BoundBinaryExpression        b => EvaluateBinaryExpression(b),
                BoundVariableExpression      n => EvaluateVariableExpression(n),
                _ => string.Empty
            };

        private static object EvaluateLiteralExpression(BoundLiteralExpression n) => n.Value;

        private object EvaluateUnaryExpression(BoundUnaryExpression e) =>
            e.Operator.Kind switch {
                BoundUnaryOperatorKind.Identity  => EvaluateExpression(e.Operand),
                BoundUnaryOperatorKind.Negation  => -(int)EvaluateExpression(e.Operand),
                BoundUnaryOperatorKind.Input     => EvaluateInput(EvaluateExpression(e.Operand)),
                _ => string.Empty
            };

        private object EvaluateBinaryExpression(BoundBinaryExpression b) {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            if ((left is string || right is string) && b.Operator.Kind == BoundBinaryOperatorKind.Addition)
                return string.Concat(left, right);
            
            return b.Operator.Kind switch {
                BoundBinaryOperatorKind.Addition                     => (int)left + (int)right,
                BoundBinaryOperatorKind.Subtraction                  => (int)left - (int)right,
                BoundBinaryOperatorKind.Multiplication               => (int)left * (int)right,
                BoundBinaryOperatorKind.Division                     => (int)left / (int)right,
                BoundBinaryOperatorKind.Modulus                      => (int)left % (int)right,
                BoundBinaryOperatorKind.Concatenation                => string.Concat(left, right),
                _ => string.Empty
            };
        }

        private object EvaluateVariableExpression(BoundVariableExpression n) {
            return _variables[n.Variable] ?? (n.Variable.Type == BoundType.Integer ? (object)0 : "");
        }

        private string EvaluateInput(object message) {
            _writer.WriteLine(message);
            _writer.Write("> ");
            return _reader.ReadLine() ?? string.Empty;
        }
    }
}