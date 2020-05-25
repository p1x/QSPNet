using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace QSP.CodeAnalysis {
    public class Binder {
        private readonly BinderDiagnosticBag _diagnostics = new BinderDiagnosticBag();
        private readonly Dictionary<string, VariableSymbol> _variableSymbols;
        
        public Binder(Dictionary<string, VariableSymbol> variableSymbols) {
            _variableSymbols = variableSymbols;
        }

        public static BoundGlobalScope BindScope(SyntaxTree tree) {
            var compilationUnit = tree.Root;

            var variableSymbols = new Dictionary<string, VariableSymbol>();
            var binder = new Binder(variableSymbols);
            var boundStatements = new List<BoundStatement>();
            foreach (var statement in compilationUnit.Statements) {
                boundStatements.Add(binder.BindStatement(statement));
            }

            var variables = binder._variableSymbols.Values.ToImmutableArray();
            var diagnostics = tree.Diagnostics.With(binder._diagnostics);
            return new BoundGlobalScope(boundStatements.ToImmutableArray(), variables, diagnostics);
        }
        
        public BoundStatement BindStatement(StatementSyntax statement) =>
            statement switch {
                AssignmentStatementSyntax x => BindAssignmentStatement(x),
                ExpressionStatementSyntax x => BindExpressionStatement(x),
                ProcedureStatementSyntax  x => BindProcedureStatement(x),
                _ => throw new ArgumentOutOfRangeException(nameof(statement))
            };

        private BoundStatement BindAssignmentStatement(AssignmentStatementSyntax statement) {
            var variable   = BindVariable(statement.Identifier);
            var expression = BindExpression(statement.Expression);
            if (expression.Kind == BoundNodeKind.ErrorExpression)
                return BoundErrorStatement.Instance;
            
            return new BoundAssignmentStatement(variable, expression);
        }

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax statement) {
            var expression = BindExpression(statement.Expression);
            if (expression.Kind == BoundNodeKind.ErrorExpression)
                return BoundErrorStatement.Instance;
            return new BoundExpressionStatement(expression);
        }

        private BoundStatement BindProcedureStatement(ProcedureStatementSyntax statement) {
            // TODO Add ability for arguments type checking even if arguments count not match  

            var procedureSymbol = ProcedureSymbol.Get(statement.Keyword.Kind);
            if (procedureSymbol == null)
                // Reported in parser 
                return BoundErrorStatement.Instance;

            if (!procedureSymbol.CanHaveModifier && statement.Modifier != null) {
                _diagnostics.ReportNotSupportedModifier();
                return BoundErrorStatement.Instance;
            }
            
            var nodeSyntaxArray = statement.Arguments.Nodes;
            if (procedureSymbol.ArgumentsTypes.Length != nodeSyntaxArray.Length) {
                _diagnostics.ReportWrongArgumentCount();
                return BoundErrorStatement.Instance;
            }

            var arguments = BindArguments(nodeSyntaxArray, procedureSymbol.ArgumentsTypes);
            return new BoundProcedureStatement(procedureSymbol, statement.Modifier?.Kind == SyntaxTokenKind.Star, arguments);
        }

        private BoundExpression BindExpression(ExpressionSyntax expression) =>
            expression switch {
                LiteralExpressionSyntax       x => BindLiteralExpression(x),
                NameExpressionSyntax          x => BindNameExpression(x),
                UnaryExpressionSyntax         x => BindUnaryExpression(x),
                BinaryExpressionSyntax        x => BindBinaryExpression(x),
                ParenthesisedExpressionSyntax x => BindParenthesisedExpression(x),
                FunctionExpressionSyntax      x => BindFuncExpression(x),
                _ => throw new ArgumentOutOfRangeException(nameof(expression))
            };

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax expression) {
            var type = BindType(expression.Token.Kind);
            var value = expression.Token.Value ?? throw new ArgumentException("Literal expression should contain value.");

            if (type == BoundType.Undefined) {
                _diagnostics.ReportUndefinedLiteralType();
                return BoundErrorExpression.Instance;
            }
            
            return new BoundLiteralExpression(type, value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax expression) {
            if (expression.Identifier.IsManufactured)
                return BoundErrorExpression.Instance;
            
            var variable = BindVariable(expression.Identifier);
            return new BoundVariableExpression(variable);
        }

        private VariableSymbol BindVariable(SyntaxToken identifierToken) {
            if (identifierToken.Kind != SyntaxTokenKind.Identifier)
                throw new ArgumentException("identifierToken.Kind != SyntaxTokenKind.Identifier", nameof(identifierToken));

            var name = identifierToken.Text.ToUpperInvariant();
            if (_variableSymbols.TryGetValue(name, out var symbol))
                return symbol;

            var type = name.StartsWith('$') ? BoundType.String : BoundType.Number;
            return _variableSymbols[name] = new VariableSymbol(name, type);
        }
        
        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax expression) {
            var operand = BindExpression(expression.Operand);
            if (operand.Kind == BoundNodeKind.ErrorExpression)
                return BoundErrorExpression.Instance;

            var @operator = BoundUnaryOperator.Bind(expression.Operator.Kind, operand.Type);
            if (@operator.Kind == BoundUnaryOperatorKind.Undefined) {
                _diagnostics.ReportUndefinedUnaryOperator();
                return BoundErrorExpression.Instance;
            }
            
            return new BoundUnaryExpression(@operator, operand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax expression) {
            var left = BindExpression(expression.Left);
            var right = BindExpression(expression.Right);
            if (left.Kind == BoundNodeKind.ErrorExpression || right.Kind == BoundNodeKind.ErrorExpression)
                return BoundErrorExpression.Instance;

            var @operator = BoundBinaryOperator.Bind(expression.Operator.Kind, left.Type, right.Type);
            if (@operator.Kind == BoundBinaryOperatorKind.Undefined) {
                _diagnostics.ReportUndefinedBinaryOperator();
                return BoundErrorExpression.Instance;
            }

            if (@operator.LeftType != BoundType.Any && left.Type != @operator.LeftType)
                left = new BoundConversionExpression(left, @operator.LeftType);

            if (@operator.RightType != BoundType.Any && right.Type != @operator.RightType)
                right = new BoundConversionExpression(right, @operator.RightType);
            
            return new BoundBinaryExpression(left, @operator, right);
        }

        private BoundExpression BindParenthesisedExpression(ParenthesisedExpressionSyntax expression) => 
            BindExpression(expression.Expression);

        private BoundExpression BindFuncExpression(FunctionExpressionSyntax expression) {
            // TODO Add ability for arguments type checking even if arguments count not match  
            
            var functionSymbol = FunctionSymbol.Get(expression.Keyword.Kind);
            if (functionSymbol == null)
                // Reported in parser 
                return BoundErrorExpression.Instance;

            var nodeSyntaxArray = expression.Arguments.Nodes;
            if (functionSymbol.ArgumentsTypes.Length != nodeSyntaxArray.Length) {
                _diagnostics.ReportWrongArgumentCount();
                return BoundErrorExpression.Instance;
            }

            var arguments = BindArguments(nodeSyntaxArray, functionSymbol.ArgumentsTypes);
            return new BoundFunctionExpression(functionSymbol, arguments);
        }

        private ImmutableArray<BoundExpression> BindArguments(ImmutableArray<ExpressionSyntax> nodeSyntaxArray, ImmutableArray<BoundType> argumentTypes) {
            var argumentsBuilder = ImmutableArray.CreateBuilder<BoundExpression>();
            for (var i = 0; i < nodeSyntaxArray.Length; i++) {
                var boundArgument = BindExpression(nodeSyntaxArray[i]);
                if (boundArgument.Kind != BoundNodeKind.ErrorExpression && boundArgument.Type != argumentTypes[i]) {
                    _diagnostics.ReportInvalidArgumentType();
                    argumentsBuilder.Add(BoundErrorExpression.Instance);
                } else {
                    argumentsBuilder.Add(boundArgument);
                }
            }

            return argumentsBuilder.ToImmutable();
        }

        private static BoundType BindType(SyntaxTokenKind kind) =>
            kind switch {
                SyntaxTokenKind.Number => BoundType.Number,
                SyntaxTokenKind.String => BoundType.String,
                _                      => throw new ArgumentException("Argument is not a literal kind.", nameof(kind))
            };
    }
}