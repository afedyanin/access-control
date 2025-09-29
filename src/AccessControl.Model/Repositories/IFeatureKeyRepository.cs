namespace AccessControl.Model.Repositories;

public interface IFeatureKeyRepository
{
    public Task<FeatureKey[]> GetAll();

    public Task<FeatureKey?> GetById(Guid id);

    public Task<bool> Save(FeatureKey featureKey);

    public Task<int> Delete(Guid id);
}
