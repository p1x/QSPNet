namespace QSP.CodeAnalysis {
    public enum BoundNodeKind {
        ErrorStatement,
        ExpressionStatement,
        AssignmentStatement,
        ProcedureStatement,

        ErrorExpression,
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        FunctionExpression,
        ConversionExpression,
        ElementAccessExpression
    }
}