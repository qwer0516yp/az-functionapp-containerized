using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Indue.Ebt.CustomerRegistration.EntraId;

public class AccessTokenExample
{
    private readonly ILogger<AccessTokenExample> _logger;
    private readonly IAccessTokenValidator _accessTokenValidator;

    public AccessTokenExample(ILogger<AccessTokenExample> logger, IAccessTokenValidator accessTokenValidator)
    {
        _logger = logger;
        _accessTokenValidator = accessTokenValidator;
    }

    [Function("AccessTokenExample")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        try 
        {
            var authHeader = req.Headers["Authorization"].FirstOrDefault();
            var token = authHeader!.Substring("Bearer ".Length).Trim();
            var principal = await _accessTokenValidator.ValidateAccessTokenAsync(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating bearer token.");
            return new UnauthorizedResult();
        }

        _logger.LogInformation("C# HTTP trigger function processed a AccessTokenExample request at {Timestamp}", DateTime.Now);
        return new OkObjectResult($"AccessTokenExample validated successfully! {DateTime.Now}");
    }
}
