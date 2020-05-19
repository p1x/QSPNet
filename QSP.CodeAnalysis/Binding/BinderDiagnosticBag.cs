namespace QSP.CodeAnalysis {
    public class BinderDiagnosticBag : DiagnosticBag {
        public const int BinderCode = 3 << 16;
        
        public BinderDiagnosticBag() : base(BinderCode) { }

        public void ReportUndefinedUnaryOperator() {
            throw new System.NotImplementedException();
        }

        public void ReportUndefinedBinaryOperator() {
            throw new System.NotImplementedException();
        }

        public void ReportUndefinedLiteralType() {
            throw new System.NotImplementedException();
        }

        public void ReportInvalidArgumentType() {
            throw new System.NotImplementedException();
        }

        public void ReportWrongArgumentCount() {
            throw new System.NotImplementedException();
        }
    }
}