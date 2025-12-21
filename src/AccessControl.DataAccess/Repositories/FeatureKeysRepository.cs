using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.DataAccess.Converters;
using AccessControl.DataAccess.Dbos;
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

    public async Task<bool> Save(FeatureKey featureKey)
    {
        using var context = await GetDbContext();

        await SaveInternal(featureKey, context);

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

    public async Task<bool> Update(FeatureKey[] changedKeys, string[] deletedKeys)
    {
        if (changedKeys.Length <= 0 && deletedKeys.Length <= 0)
        {
            return true;
        }

        using var context = await GetDbContext();

        foreach (var featureKey in changedKeys)
        {
            await SaveInternal(featureKey, context);
        }

        var savedRows = await context.SaveChangesAsync();

        var deletedRows = await context.FeatureKeys
            .Where(s => deletedKeys.Contains(s.Name))
            .ExecuteDeleteAsync();

        return (savedRows > 0) || (deletedRows > 0);
    }

    private async Task SaveInternal(FeatureKey featureKey, AccessControlDbContext context)
    {
        var rolePermissionsDict = featureKey.RolePermissions.ToDictionary(rp => rp.RoleName);

        var rolesToSave = await context
            .Roles
            .Where(r => rolePermissionsDict.Keys.Contains(r.Name))
            .ToArrayAsync();

        var existingFk = await context
            .FeatureKeys
            .Include(fk => fk.FeatureKeyRoles)
            .FirstOrDefaultAsync(p => p.Name == featureKey.Name);

        if (existingFk == null)
        {
            var newFk = new FeatureKeyDbo
            {
                Name = featureKey.Name,
            };

            foreach (var role in rolesToSave)
            {
                if (!rolePermissionsDict.TryGetValue(role.Name, out var rolePermissions))
                {
                    continue;
                }

                var fkRole = new FeatureKeyRoleDbo
                {
                    FeatureKey = newFk,
                    FeatureKeyName = newFk.Name,
                    Role = role,
                    RoleName = role.Name,
                    Permissions = rolePermissions.Permissions,
                };

                newFk.Roles.Add(role);
                newFk.FeatureKeyRoles.Add(fkRole);
            }

            context.FeatureKeys.Add(newFk);
        }
        else
        {
            existingFk.FeatureKeyRoles.Clear();
            foreach (var role in rolesToSave)
            {
                if (!rolePermissionsDict.TryGetValue(role.Name, out var rolePermissions))
                {
                    continue;
                }

                var fkRole = new FeatureKeyRoleDbo
                {
                    FeatureKey = existingFk,
                    FeatureKeyName = existingFk.Name,
                    Role = role,
                    RoleName = role.Name,
                    Permissions = rolePermissions.Permissions,
                };

                existingFk.FeatureKeyRoles.Add(fkRole);
            }
        }
    }
}
