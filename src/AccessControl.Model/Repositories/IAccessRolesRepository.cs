namespace AccessControl.Model.Repositories;

public interface IAccessRolesRepository
{
    public Task<AccessRole[]> GetAll();

    public Task<AccessRole?> GetById(Guid id);

    public Task<bool> Save(AccessRole role);

    public Task<int> Delete(Guid id);
}
