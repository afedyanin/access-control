using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi.Authorization;
public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}
