using AccessControl.Contracts.Entities;

namespace AccessControl.Contracts.Repositories;

public interface IResourcesRepository
{
    public Task<Resource[]> GetAll();

    public Task<Resource?> GetById(Guid id);

    public Task<bool> Save(Resource resource);

    public Task<int> Delete(Guid id);
}
