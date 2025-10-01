using AccessControl.Model;
using AccessControl.Model.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess.Repositories;
internal class FeatureKeysRepository : RepositoryBase, IFeatureKeysRepository
{
    public FeatureKeysRepository(IDbContextFactory<AccessControlDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<FeatureKey[]> GetAll()
    {
        using var context = await GetDbContext();

        return await context
            .FeatureKeys
            .OrderBy(x => x.Name)
            .ToArrayAsync();
    }

    public async Task<FeatureKey?> GetByName(string name)
    {
        using var context = await GetDbContext();

        return await context
            .FeatureKeys
            .SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<bool> Save(FeatureKey featureKey)
    {
        using var context = await GetDbContext();

        var existing = await context
            .FeatureKeys
            .FirstOrDefaultAsync(p => p.Name == featureKey.Name);

        if (existing != null)
        {
            existing.FeatureKeyRoles.Clear();
            foreach (var fkRole in featureKey.FeatureKeyRoles)
            {
                existing.FeatureKeyRoles.Add(fkRole);
            }
        }
        else
        {
            context.FeatureKeys.Add(featureKey);
        }

        var savedRecords = await context.SaveChangesAsync();

        return savedRecords > 0;
    }

    public async Task<int> Delete(string name)
    {
        using var context = await GetDbContext();

        return await context.FeatureKeys
            .Where(s => s.Name == name)
            .ExecuteDeleteAsync();
    }
}
