using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Requests;
using Refit;

namespace AccessControl.Contracts;

public interface IAccessControlAdminClient : IAccessControlClient
{
    #region FeatureKeyPermissionsController

    [Post("/api/feature-key-permissions/{fkName}")]
    public Task<FeatureKey> CreatePermissions(string fkName, [Body] PermissionsRequest request);

    [Put("/api/feature-key-permissions/{fkName}/role/{roleName}/{permissions}")]
    public Task<FeatureKey> UpdatePermissions(string fkName, string roleName, Permissions permissions);

    #endregion

    #region ResourcePermissionsController

    [Post("/api/resource-permissions/{id}")]
    public Task<Resource> CreateResourcePermissions(Guid id, [Body] PermissionsRequest request);

    [Put("/api/resource-permissions/{id}/role/{roleName}/{permissions}")]
    public Task<Resource> UpdateResourcePermissions(Guid id, string roleName, Permissions permissions);

    #endregion

    #region FeatureKeysController

    [Get("/api/feature-keys")]
    public Task<FeatureKey[]> GetAllFeatureKeys();

    [Get("/api/feature-keys/{name}")]
    public Task<FeatureKey> GetFeatureKeyByName(string name);

    [Post("/api/feature-keys")]
    public Task<FeatureKey> CreateFeatureKey([Body] FeatureKeyRequest request);

    [Delete("/api/feature-keys/{name}")]
    public Task<int> DeleteFeatureKey(string name);

    #endregion

    #region ResourcesController

    [Get("/api/resources")]
    public Task<Resource[]> GetAllResources();

    [Get("/api/resources/{id}")]
    public Task<Resource[]> GetResourceById(Guid id);

    [Post("/api/resources")]
    public Task<Resource> CreateResource([Body] ResourceRequest request);

    [Delete("/api/resources/{id}")]
    public Task<Resource[]> DeleteResource(Guid id);

    #endregion

    #region RolesController

    [Get("/api/roles")]
    public Task<Role[]> GetAllRoles();

    [Get("/api/roles/{name}")]
    public Task<Role> GetRoleByName(string name);

    [Post("/api/roles")]
    public Task<Role> CreateRole([Body] Role request);

    [Put("/api/roles")]
    public Task<Role> UpdateRole([Body] Role request);

    [Delete("/api/roles/{name}")]
    public Task<int> DeleteRole(string name);

    #endregion

    #region UsersController

    [Post("/api/users")]
    public Task<User> CreateUser([Body] User userDto);

    [Get("/api/users")]
    public Task<User[]> GetAllUsers();

    [Get("/api/users/{name}")]
    public Task<User> GetByName(string name);

    [Delete("/api/users/{name}")]
    public Task<int> DeleteUser(string name);

    #endregion
}
