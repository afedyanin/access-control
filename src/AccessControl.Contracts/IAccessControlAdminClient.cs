using AccessControl.Contracts.Requests;
using Refit;

namespace AccessControl.Contracts;

public interface IAccessControlAdminClient : IAccessControlClient
{
    #region FeatureKeyPermissionsController

    [Post("/api/feature-key-permissions/{fkName}")]
    public Task<FeatureKeyDto> CreatePermissions(string fkName, [Body] PermissionsRequest request);

    [Put("/api/feature-key-permissions/{fkName}/role/{roleName}/{permissions}")]
    public Task<FeatureKeyDto> UpdatePermissions(string fkName, string roleName, Permissions permissions);

    #endregion

    #region ResourcePermissionsController

    [Post("/api/resource-permissions/{id}")]
    public Task<ResourceDto> CreateResourcePermissions(Guid id, [Body] PermissionsRequest request);

    [Put("/api/resource-permissions/{id}/role/{roleName}/{permissions}")]
    public Task<ResourceDto> UpdateResourcePermissions(Guid id, string roleName, Permissions permissions);

    #endregion

    #region FeatureKeysController

    [Get("/api/feature-keys")]
    public Task<FeatureKeyDto[]> GetAllFeatureKeys();

    [Get("/api/feature-keys/{name}")]
    public Task<FeatureKeyDto> GetFeatureKeyByName(string name);

    [Post("/api/feature-keys")]
    public Task<FeatureKeyDto> CreateFeatureKey([Body] FeatureKeyRequest request);

    [Delete("/api/feature-keys/{name}")]
    public Task<int> DeleteFeatureKey(string name);

    #endregion

    #region ResourcesController

    [Get("/api/resources")]
    public Task<ResourceDto[]> GetAllResources();

    [Get("/api/resources/{id}")]
    public Task<ResourceDto[]> GetResourceById(Guid id);

    [Post("/api/resources")]
    public Task<ResourceDto> CreateResource([Body] ResourceRequest request);

    [Delete("/api/resources/{id}")]
    public Task<ResourceDto[]> DeleteResource(Guid id);

    #endregion

    #region RolesController

    [Get("/api/roles")]
    public Task<RoleDto[]> GetAllRoles();

    [Get("/api/roles/{name}")]
    public Task<RoleDto> GetRoleByName(string name);

    [Post("/api/roles")]
    public Task<RoleDto> CreateRole([Body] RoleRequest request);

    [Put("/api/roles")]
    public Task<RoleDto> UpdateRole([Body] RoleRequest request);

    [Delete("/api/roles/{name}")]
    public Task<int> DeleteRole(string name);

    #endregion

    #region UsersController

    [Post("/api/users")]
    public Task<UserDto> CreateUser([Body] UserDto userDto);

    [Get("/api/users")]
    public Task<UserDto[]> GetAllUsers();

    [Get("/api/users/{name}")]
    public Task<UserDto> GetByName(string name);

    [Delete("/api/users/{name}")]
    public Task<int> DeleteUser(string name);

    #endregion
}
