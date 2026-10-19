using AwsCognito.Test.Domain.Exceptions;

namespace AwsCognito.Test.Application.Exceptions
{
    public class SampleAlreadyExistsException(string message)
        : AppException(ErrorKind.Conflict, "sample.already_exists", message);
}
