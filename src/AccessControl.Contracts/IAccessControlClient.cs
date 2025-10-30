using AccessControl.Contracts.Entities;
using Refit;

namespace AccessControl.Contracts;

public interface IAccessControlClient
{
    // Roles API

    [Get("/api/roles")]
    public Task<Role[]> GetAllRoles();

    [Get("/api/roles/{name}")]
    public Task<Role> GetRoleByName(string name);

    [Post("/api/roles")]
    public Task<Role> CreateRole(Role request);

    [Delete("/api/roles/{name}")]
    public Task<int> DeleteRole(string name);

    // Users API

    [Post("/api/users")]
    public Task<User> CreateUser(User userDto);

    [Get("/api/users")]
    public Task<User[]> GetAllUsers();

    [Get("/api/users/{name}")]
    public Task<User> GetUserByName(string name);

    [Delete("/api/users/{name}")]
    public Task<int> DeleteUser(string name);


    // Feature keys API

    [Get("/api/feature-keys")]
    public Task<FeatureKey[]> GetAllFeatureKeys();

    [Get("/api/feature-keys/{name}")]
    public Task<FeatureKey> GetFeatureKeyByName(string name);

    [Post("/api/feature-keys")]
    public Task<FeatureKey> CreateFeatureKey(FeatureKey featureKey);

    [Delete("/api/feature-keys/{name}")]
    public Task<int> DeleteFeatureKey(string name);


    // Resources API

    [Get("/api/resources")]
    public Task<Resource[]> GetAllResources();

    [Get("/api/resources/{id}")]
    public Task<Resource> GetResourceById(Guid id);

    [Post("/api/resources")]
    public Task<Resource> CreateResource(Resource resource);

    [Delete("/api/resources/{id}")]
    public Task<Resource[]> DeleteResource(Guid id);
}
