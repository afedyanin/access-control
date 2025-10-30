using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.DataAccess.Converters;
using AccessControl.DataAccess.Dbos;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess.Repositories;

internal class ResourcesRepository : RepositoryBase, IResourcesRepository
{
    public ResourcesRepository(IDbContextFactory<AccessControlDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<Resource[]> GetAll()
    {
        using var context = await GetDbContext();

        var res = await context
            .Resources
            .Include(fk => fk.ResourceRoles)
            .OrderBy(x => x.Name)
            .ToArrayAsync();

        return res.ToEntity();
    }

    public async Task<Resource?> GetById(Guid id)
    {
        using var context = await GetDbContext();

        var res = await context
            .Resources
            .Include(fk => fk.ResourceRoles)
            .SingleOrDefaultAsync(x => x.Id == id);

        return res?.ToEntity();
    }

    public async Task<bool> Save(Resource resource)
    {
        using var context = await GetDbContext();

        var rolePermissionsDict = resource.RolePermissions.ToDictionary(rp => rp.RoleName);

        var rolesToSave = await context
            .Roles
            .Where(r => rolePermissionsDict.Keys.Contains(r.Name))
            .ToArrayAsync();

        var existingRes = await context
            .Resources
            .Include(fk => fk.ResourceRoles)
            .FirstOrDefaultAsync(p => p.Id == resource.Id);

        if (existingRes == null)
        {
            var newRes = new ResourceDbo
            {
                Id = resource.Id,
                Name = resource.Name,
            };

            foreach (var role in rolesToSave)
            {
                if (!rolePermissionsDict.TryGetValue(role.Name, out var rolePermissions))
                {
                    continue;
                }

                var resRole = new ResourceRoleDbo
                {
                    Resource = newRes,
                    ResourceId = newRes.Id,
                    Role = role,
                    RoleName = role.Name,
                    Permissions = rolePermissions.Permissions,
                };

                newRes.Roles.Add(role);
                newRes.ResourceRoles.Add(resRole);
            }

            context.Resources.Add(newRes);
        }
        else
        {
            existingRes.ResourceRoles.Clear();
            foreach (var role in rolesToSave)
            {
                if (!rolePermissionsDict.TryGetValue(role.Name, out var rolePermissions))
                {
                    continue;
                }

                var resRole = new ResourceRoleDbo
                {
                    Resource = existingRes,
                    ResourceId = existingRes.Id,
                    Role = role,
                    RoleName = role.Name,
                    Permissions = rolePermissions.Permissions,
                };

                existingRes.ResourceRoles.Add(resRole);
            }
        }

        var savedRecords = await context.SaveChangesAsync();

        return savedRecords > 0;
    }

    public async Task<int> Delete(Guid id)
    {
        using var context = await GetDbContext();

        return await context.Resources
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();
    }
}
