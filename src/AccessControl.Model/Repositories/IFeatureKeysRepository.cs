namespace AccessControl.Model.Repositories;

public interface IFeatureKeysRepository
{
    public Task<FeatureKey[]> GetAll();

    public Task<FeatureKey?> GetByName(string name);

    public Task<bool> Save(FeatureKey featureKey);

    public Task<int> Delete(string name);
}
