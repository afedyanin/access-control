namespace AccessControl.Model.Repositories;

public interface IRolesRepository
{
    public Task<Role[]> GetAll();

    public Task<Role?> GetByName(string name);

    public Task<bool> Save(Role role);

    public Task<int> Delete(string name);
}
