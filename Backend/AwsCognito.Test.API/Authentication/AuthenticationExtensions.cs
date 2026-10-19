using AwsCognito.Test.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AwsCognito.Test.API.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddCognitoAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cognito = configuration.GetSection(CognitoOptions.SectionName).Get<CognitoOptions>()
            ?? throw new InvalidOperationException($"Missing '{CognitoOptions.SectionName}' configuration section.");

        services
            .AddOptions<CognitoOptions>()
            .Bind(configuration.GetSection(CognitoOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Signing keys are discovered from {Authority}/.well-known/openid-configuration.
                options.Authority = cognito.Issuer;

                // Keep claim names as they appear in the token (sub, cognito:groups, ...).
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = cognito.Issuer,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),

                    // Cognito access tokens carry 'client_id' instead of 'aud'; checked in OnTokenValidated.
                    ValidateAudience = false
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var principal = context.Principal;

                        var isAccessToken = principal?.FindFirst("token_use")?.Value == "access";
                        var clientId = principal?.FindFirst("client_id")?.Value;

                        if (!isAccessToken || clientId != cognito.ClientId)
                        {
                            context.Fail("Token is not an access token issued for this application.");
                        }

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        // Replace the default empty 401 with a ProblemDetails body.
                        context.HandleResponse();

                        var (code, message) = context.AuthenticateFailure switch
                        {
                            null => (AuthErrors.MissingTokenCode, AuthErrors.MissingTokenMessage),
                            SecurityTokenExpiredException => (AuthErrors.TokenExpiredCode, AuthErrors.TokenExpiredMessage),
                            _ => (AuthErrors.InvalidTokenCode, AuthErrors.InvalidTokenMessage)
                        };

                        context.Response.Headers.WWWAuthenticate = context.AuthenticateFailure is null
                            ? "Bearer"
                            : "Bearer error=\"invalid_token\"";

                        return AuthErrors.WriteProblem(
                            context.HttpContext, StatusCodes.Status401Unauthorized, ErrorKind.Unauthorized, code, message);
                    },

                    OnForbidden = context =>
                        AuthErrors.WriteProblem(
                            context.HttpContext,
                            StatusCodes.Status403Forbidden,
                            ErrorKind.Forbidden,
                            AuthErrors.ForbiddenCode,
                            AuthErrors.ForbiddenMessage)
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddFrontendCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

        return services;
    }
}
