using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.DataAccess.Converters;
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

    public Task<bool> Save(Resource resource)
    {
        // TODO: Fix it
        return Task.FromResult(false);
        /*
        using var context = await GetDbContext();

        var existing = await context
            .Resources
            .FirstOrDefaultAsync(p => p.Id == resource.Id);

        if (existing != null)
        {
            existing.Roles.Clear();
            foreach (var role in resource.Roles)
            {
                context.Attach(role);
                existing.Roles.Add(role);
            }

            existing.ResourceRoles.Clear();
            foreach (var rRole in resource.ResourceRoles)
            {
                context.Attach(rRole.Role);
                existing.ResourceRoles.Add(rRole);
            }
        }
        else
        {
            context.Resources.Add(resource);
        }

        var savedRecords = await context.SaveChangesAsync();

        return savedRecords > 0;
        */
    }

    public async Task<int> Delete(Guid id)
    {
        using var context = await GetDbContext();

        return await context.Resources
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();
    }
}
