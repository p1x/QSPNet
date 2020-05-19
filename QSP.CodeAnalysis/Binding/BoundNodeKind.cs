namespace QSP.CodeAnalysis {
    public enum BoundNodeKind {
        ErrorStatement,
        ExpressionStatement,
        AssignmentStatement,

        ErrorExpression,
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        FunctionExpression
    }
}