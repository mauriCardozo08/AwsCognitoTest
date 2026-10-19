using System.ComponentModel.DataAnnotations;

namespace AwsCognito.Test.API.Authentication;

public sealed class CognitoOptions
{
    public const string SectionName = "Cognito";

    [Required]
    public string Region { get; init; } = string.Empty;

    [Required]
    public string UserPoolId { get; init; } = string.Empty;

    [Required]
    public string ClientId { get; init; } = string.Empty;

    public string Issuer => $"https://cognito-idp.{Region}.amazonaws.com/{UserPoolId}";
}
