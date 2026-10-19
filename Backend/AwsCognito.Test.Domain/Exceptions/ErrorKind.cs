namespace AwsCognito.Test.Domain.Exceptions
{
    /// <summary>
    /// Transport-agnostic error category. The API layer maps each kind to an HTTP status in one place.
    /// </summary>
    public enum ErrorKind
    {
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Forbidden
    }
}
