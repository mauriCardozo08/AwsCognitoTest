using AwsCognito.Test.Domain.Exceptions;

namespace AwsCognito.Test.Application.Exceptions
{
    public class SampleNotFoundException(string message)
        : AppException(ErrorKind.NotFound, "sample.not_found", message);
}
