using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccessControl.WebApi.Authorization;

// https://www.milanjovanovic.tech/blog/how-to-implement-api-key-authentication-in-aspnet-core
public class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-API-Key";

    private readonly IApiKeyValidator _apiKeyValidator;

    public ApiKeyAuthorizationFilter(IApiKeyValidator apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string? apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (apiKey == null || !_apiKeyValidator.IsValid(apiKey))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
