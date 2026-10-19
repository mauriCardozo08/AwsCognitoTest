using AwsCognito.Test.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AwsCognito.Test.API.Exceptions;

public sealed class AppExceptionHandler(ILogger<AppExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Unexpected errors must not leak internal details to the client.
        var problem = exception is AppException appException
            ? new ProblemDetails
            {
                Status = ToStatusCode(appException.Kind),
                Type = appException.Code,
                Title = appException.Kind.ToString(),
                Detail = appException.Message
            }
            : new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred."
            };

        if (exception is not AppException)
            logger.LogError(exception, "Unhandled exception");

        httpContext.Response.StatusCode = problem.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    // The only place that maps error categories to HTTP. New exceptions never touch this.
    private static int ToStatusCode(ErrorKind kind) => kind switch
    {
        ErrorKind.Validation => StatusCodes.Status400BadRequest,
        ErrorKind.NotFound => StatusCodes.Status404NotFound,
        ErrorKind.Conflict => StatusCodes.Status409Conflict,
        ErrorKind.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorKind.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };
}
