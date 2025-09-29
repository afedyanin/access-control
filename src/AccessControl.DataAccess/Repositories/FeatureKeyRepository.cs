using AccessControl.Model;
using AccessControl.Model.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess.Repositories;
internal class FeatureKeyRepository : RepositoryBase, IFeatureKeyRepository
{
    public FeatureKeyRepository(IDbContextFactory<AccessControlDbContext> contextFactory) : base(contextFactory)
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

    public async Task<FeatureKey?> GetById(Guid id)
    {
        using var context = await GetDbContext();

        return await context
            .FeatureKeys
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> Save(FeatureKey featureKey)
    {
        using var context = await GetDbContext();

        var existing = await context
            .FeatureKeys
            .FirstOrDefaultAsync(p => p.Id == featureKey.Id);

        if (existing != null)
        {
            existing.Name = featureKey.Name;
            existing.AccessRoles.Clear();

            foreach (var role in featureKey.AccessRoles)
            {
                existing.AccessRoles.Add(role);
            }

            existing.FeatureKeyAccessRoles.Clear();
            foreach (var fkRole in featureKey.FeatureKeyAccessRoles)
            {
                existing.FeatureKeyAccessRoles.Add(fkRole);
            }
        }
        else
        {
            context.FeatureKeys.Add(featureKey);
        }

        var savedRecords = await context.SaveChangesAsync();

        return savedRecords > 0;
    }

    public async Task<int> Delete(Guid id)
    {
        using var context = await GetDbContext();

        return await context.FeatureKeys
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();
    }
}
