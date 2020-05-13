namespace QSPNet.Interpreter.Binding {
    public enum BoundNodeKind {
        ErrorStatement,
        ExpressionStatement,
        AssignmentStatement,

        ErrorExpression,
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
    }
}