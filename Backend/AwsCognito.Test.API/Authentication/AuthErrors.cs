using AwsCognito.Test.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AwsCognito.Test.API.Authentication;

/// <summary>
/// Authentication and authorization failures are produced by the ASP.NET Core middleware, not by
/// <see cref="AppException"/>s, so they never reach the exception handler. They are written here with
/// the same <see cref="ProblemDetails"/> shape (Type = code, Title = kind, Detail = message).
/// </summary>
public static class AuthErrors
{
    public const string MissingTokenCode = "auth.missing_token";
    public const string MissingTokenMessage = "Authentication is required. No access token was provided.";

    public const string TokenExpiredCode = "auth.token_expired";
    public const string TokenExpiredMessage = "The access token has expired.";

    public const string InvalidTokenCode = "auth.invalid_token";
    public const string InvalidTokenMessage = "The access token is not valid.";

    public const string ForbiddenCode = "auth.forbidden";
    public const string ForbiddenMessage = "You do not have permission to perform this action.";

    public static Task WriteProblem(
        HttpContext httpContext,
        int statusCode,
        ErrorKind kind,
        string code,
        string message)
    {
        httpContext.Response.StatusCode = statusCode;

        return httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Type = code,
            Title = kind.ToString(),
            Detail = message
        });
    }
}
