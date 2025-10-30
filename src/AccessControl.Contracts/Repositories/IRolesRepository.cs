using AccessControl.Contracts.Entities;

namespace AccessControl.Contracts.Repositories;

public interface IRolesRepository
{
    public Task<Role[]> GetAll();

    public Task<Role?> GetByName(string name);

    public Task<Role[]> GetByNames(string[] roleNames);

    public Task<bool> Save(Role role);

    public Task<int> Delete(string name);
}
