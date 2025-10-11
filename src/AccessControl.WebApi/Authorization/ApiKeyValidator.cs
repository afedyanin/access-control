namespace AccessControl.WebApi.Authorization;
public class ApiKeyValidator : IApiKeyValidator
{
    public bool IsValid(string apiKey)
    {
        // Implement logic for validating the API key.
        return false;
    }
}

public interface IApiKeyValidator
{
    public bool IsValid(string apiKey);
}
