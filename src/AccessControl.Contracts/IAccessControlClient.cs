using Refit;

namespace AccessControl.Contracts;
public interface IAccessControlClient
{
    #region FeatureKeyPermissionsController

    [Get("/api/feature-key-permissions/all")]
    public Task<Dictionary<string, Permissions>> GetAllFeatureKeysPermissions([Query] string[] roleNames);

    [Get("/api/feature-key-permissions/{fkName}")]
    public Task<Permissions> GetEffectivePermissions(string fkName, [Query] string[] roleNames);

    #endregion

    #region ResourcePermissionsController

    [Get("/api/resource-permissions/all")]
    public Task<Dictionary<string, Permissions>> GetAllResourcesPermissions([Query] string[] roleNames);

    [Get("/api/resource-permissions/{id}")]
    public Task<Permissions> GetEffectiveResourcePermissions(Guid id, [Query] string[] roleNames);

    #endregion
}
