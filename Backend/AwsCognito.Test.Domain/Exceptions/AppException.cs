namespace AwsCognito.Test.Domain.Exceptions
{
    /// <summary>
    /// Base for every expected (business) error. Declare the <see cref="ErrorKind"/> and a stable
    /// <see cref="Code"/> once, in the exception itself; the API never needs to know individual types.
    /// </summary>
    public abstract class AppException : Exception
    {
        protected AppException(ErrorKind kind, string code, string message) : base(message)
        {
            Kind = kind;
            Code = code;
        }

        public ErrorKind Kind { get; }

        /// <summary>Stable, machine-readable code for clients (e.g. "sample.not_found").</summary>
        public string Code { get; }
    }
}
