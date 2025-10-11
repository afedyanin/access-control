using Microsoft.Extensions.Configuration;

namespace AccessControl.WebApi.Authorization;
public class ApiKeyValidator : IApiKeyValidator
{
    private readonly IConfiguration _configuration;

    public ApiKeyValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool IsValid(string userApiKey)
    {
        if (string.IsNullOrWhiteSpace(userApiKey))
        {
            return false;
        }

        var apiKey = _configuration.GetSection(ApiKeyConsts.ApiKeyName).Value;

        if (apiKey == null || apiKey != userApiKey)
        {
            return false;
        }

        return true;
    }
}

public interface IApiKeyValidator
{
    public bool IsValid(string userApiKey);
}
