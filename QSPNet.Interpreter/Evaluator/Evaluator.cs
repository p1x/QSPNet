﻿using System;
using System.Collections.Generic;

namespace QSPNet.Interpreter {
    public class Evaluator {
        private readonly StatementSyntax _statement;
        private readonly Dictionary<string, object> _variables;
        
        public Evaluator(StatementSyntax expression, Dictionary<string, object> variables) {
            _statement = expression;
            _variables = variables;
        }

        public EvaluationResult Evaluate() =>
            _statement switch {
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
                NumberExpressionSyntax n => EvaluateNumberExpression(n),
                UnaryExpressionSyntax  u => EvaluateUnaryExpression(u),
                BinaryExpressionSyntax b => EvaluateBinaryExpression(b),
                NameExpressionSyntax  nn => EvaluateNameExpression(nn),
                _ => string.Empty
            };

        private static object EvaluateNumberExpression(NumberExpressionSyntax n) => 
            n.Token.Value!;

        private object EvaluateUnaryExpression(UnaryExpressionSyntax u) =>
            u.Operator.Kind switch {
                SyntaxTokenKind.Plus => EvaluateExpression(u.Operand),
                SyntaxTokenKind.Minus => -(int)EvaluateExpression(u.Operand),
                _ => string.Empty
            };

        private object EvaluateBinaryExpression(BinaryExpressionSyntax b) {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);
            return b.Operator.Kind switch {
                SyntaxTokenKind.Plus => (int)left + (int)right,
                SyntaxTokenKind.Minus => (int)left - (int)right,
                SyntaxTokenKind.Slash => (int)left / (int)right,
                SyntaxTokenKind.Star => (int)left * (int)right,
                SyntaxTokenKind.Mod => (int)left % (int)right,
                _ => string.Empty
            };
        }

        private object EvaluateNameExpression(NameExpressionSyntax n) {
            return _variables[n.Identifier.Text];
        }
    }
}