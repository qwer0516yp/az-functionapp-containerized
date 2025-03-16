using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Indue.Ebt.CustomerRegistration.EntraId;

public class AccessTokenValidator: IAccessTokenValidator
{
    //https://login.microsoftonline.com/{tenant}/v2.0
    public string? Authority { get; set; }
    //Azure AD app registration client ID {your-client-id}
    public string? Audience { get; set; }

    public async Task<ClaimsPrincipal> ValidateAccessTokenAsync(string token)
    {
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            $"{Authority!}/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever());

        var openIdConfig = await configurationManager.GetConfigurationAsync();
        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = Authority!,
            ValidAudience = Audience!,
            IssuerSigningKeys = openIdConfig.SigningKeys,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

        return principal;
    }
}

public interface IAccessTokenValidator
{
    Task<ClaimsPrincipal> ValidateAccessTokenAsync(string token);
}
