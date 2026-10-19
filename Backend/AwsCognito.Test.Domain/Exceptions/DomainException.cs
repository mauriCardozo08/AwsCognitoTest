namespace AwsCognito.Test.Domain.Exceptions
{
    public class DomainException : AppException
    {
        public DomainException(string message)
            : base(ErrorKind.Validation, "domain.error", message) { }

        protected DomainException(string code, string message)
            : base(ErrorKind.Validation, code, message) { }
    }
}
