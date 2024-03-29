﻿using System;
using System.Collections.Generic;

namespace QSP.CodeAnalysis {
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
    public class ElementAccessExpressionSyntax : ExpressionSyntax {
        public ElementAccessExpressionSyntax(NameExpressionSyntax name, SyntaxToken openBracket, ExpressionSyntax? argument, SyntaxToken closeBracket) {
                Name = name ?? throw new ArgumentNullException(nameof(name)); 
                OpenBracket = openBracket ?? throw new ArgumentNullException(nameof(openBracket)); 
                Argument = argument; 
                CloseBracket = closeBracket ?? throw new ArgumentNullException(nameof(closeBracket)); 
        }

        public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.ElementAccess;

        public NameExpressionSyntax Name { get; }
        public SyntaxToken OpenBracket { get; }
        public ExpressionSyntax? Argument { get; }
        public SyntaxToken CloseBracket { get; }

        public override IEnumerable<object> GetChildren() {
            yield return Name;
            yield return OpenBracket;
            if (Argument != null)  
                yield return Argument;
            yield return CloseBracket;
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
    public class FunctionExpressionSyntax : ExpressionSyntax {
        public FunctionExpressionSyntax(SyntaxToken keyword, SyntaxToken? openParenthesis, SeparatedListSyntax<ExpressionSyntax> arguments, SyntaxToken? closeParenthesis) {
                Keyword = keyword ?? throw new ArgumentNullException(nameof(keyword)); 
                OpenParenthesis = openParenthesis; 
                Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments)); 
                CloseParenthesis = closeParenthesis; 
        }

        public override SyntaxExpressionKind ExpressionKind => SyntaxExpressionKind.Function;

        public SyntaxToken Keyword { get; }
        public SyntaxToken? OpenParenthesis { get; }
        public SeparatedListSyntax<ExpressionSyntax> Arguments { get; }
        public SyntaxToken? CloseParenthesis { get; }

        public override IEnumerable<object> GetChildren() {
            yield return Keyword;
            if (OpenParenthesis != null)  
                yield return OpenParenthesis;
            foreach (var item in Arguments)
                yield return item;
            if (CloseParenthesis != null)  
                yield return CloseParenthesis;
        }   
    }
    public class ExpressionStatementSyntax : StatementSyntax {
        public ExpressionStatementSyntax(ExpressionSyntax expression, SyntaxToken endToken) : base(endToken)  {
                Expression = expression ?? throw new ArgumentNullException(nameof(expression)); 
        }

        public override SyntaxStatementKind StatementKind => SyntaxStatementKind.Expression;

        public ExpressionSyntax Expression { get; }

        public override IEnumerable<object> GetChildren() {
            yield return Expression;
        }   
    }
    public class AssignmentStatementSyntax : StatementSyntax {
        public AssignmentStatementSyntax(ExpressionSyntax variable, SyntaxToken equals, ExpressionSyntax expression, SyntaxToken endToken) : base(endToken)  {
                Variable = variable ?? throw new ArgumentNullException(nameof(variable)); 
                Equals = equals ?? throw new ArgumentNullException(nameof(equals)); 
                Expression = expression ?? throw new ArgumentNullException(nameof(expression)); 
        }

        public override SyntaxStatementKind StatementKind => SyntaxStatementKind.Assignment;

        public ExpressionSyntax Variable { get; }
        public SyntaxToken Equals { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<object> GetChildren() {
            yield return Variable;
            yield return Equals;
            yield return Expression;
        }   
    }
    public class ProcedureStatementSyntax : StatementSyntax {
        public ProcedureStatementSyntax(SyntaxToken? modifier, SyntaxToken keyword, SyntaxToken? openParenthesis, SeparatedListSyntax<ExpressionSyntax> arguments, SyntaxToken? closeParenthesis, SyntaxToken endToken) : base(endToken)  {
                Modifier = modifier; 
                Keyword = keyword ?? throw new ArgumentNullException(nameof(keyword)); 
                OpenParenthesis = openParenthesis; 
                Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments)); 
                CloseParenthesis = closeParenthesis; 
        }

        public override SyntaxStatementKind StatementKind => SyntaxStatementKind.Procedure;

        public SyntaxToken? Modifier { get; }
        public SyntaxToken Keyword { get; }
        public SyntaxToken? OpenParenthesis { get; }
        public SeparatedListSyntax<ExpressionSyntax> Arguments { get; }
        public SyntaxToken? CloseParenthesis { get; }

        public override IEnumerable<object> GetChildren() {
            if (Modifier != null)  
                yield return Modifier;
            yield return Keyword;
            if (OpenParenthesis != null)  
                yield return OpenParenthesis;
            foreach (var item in Arguments)
                yield return item;
            if (CloseParenthesis != null)  
                yield return CloseParenthesis;
        }   
    }
}

