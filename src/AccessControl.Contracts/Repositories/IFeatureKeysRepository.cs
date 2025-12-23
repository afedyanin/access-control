using AccessControl.Contracts.Entities;

namespace AccessControl.Contracts.Repositories;

public interface IFeatureKeysRepository
{
    public Task<FeatureKey[]> GetAll();

    public Task<FeatureKey?> GetByName(string name);

    public Task<bool> Save(FeatureKey featureKey);

    public Task<int> Update(FeatureKey[] changedKeys, string[] deletedKeys);

    public Task<int> Delete(string name);
}
