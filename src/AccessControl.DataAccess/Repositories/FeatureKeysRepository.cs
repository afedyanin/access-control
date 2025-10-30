using System.Data;
using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.DataAccess.Converters;
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

        var res = await context
            .FeatureKeys
            .Include(fk => fk.FeatureKeyRoles)
            .OrderBy(x => x.Name)
            .ToArrayAsync();

        return res.ToEntity();
    }

    public async Task<FeatureKey?> GetByName(string name)
    {
        using var context = await GetDbContext();

        var res = await context
            .FeatureKeys
            .Include(fk => fk.FeatureKeyRoles)
            .SingleOrDefaultAsync(x => x.Name == name);

        return res?.ToEntity();
    }

    public Task<bool> Save(FeatureKey featureKey)
    {
        // TODO: Fix it
        return Task.FromResult(false);

        /*
        using var context = await GetDbContext();

        var existing = await context
            .FeatureKeys
            .FirstOrDefaultAsync(p => p.Name == featureKey.Name);

        if (existing != null)
        {
            existing.Roles.Clear();
            foreach (var role in featureKey.Roles)
            {
                context.Attach(role);
                existing.Roles.Add(role);
            }

            existing.FeatureKeyRoles.Clear();
            foreach (var fkRole in featureKey.FeatureKeyRoles)
            {
                context.Attach(fkRole.Role);
                existing.FeatureKeyRoles.Add(fkRole);
            }
        }
        else
        {
            context.FeatureKeys.Add(featureKey);
        }

        var savedRecords = await context.SaveChangesAsync();

        return savedRecords > 0;
        */
    }

    public async Task<int> Delete(string name)
    {
        using var context = await GetDbContext();

        return await context.FeatureKeys
            .Where(s => s.Name == name)
            .ExecuteDeleteAsync();
    }
}
