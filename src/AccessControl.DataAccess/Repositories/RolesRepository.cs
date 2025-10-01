using AccessControl.Model;
using AccessControl.Model.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess.Repositories;

internal class RolesRepository : RepositoryBase, IRolesRepository
{
    public RolesRepository(IDbContextFactory<AccessControlDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<Role[]> GetAll()
    {
        using var context = await GetDbContext();

        return await context
            .Roles
            .OrderBy(x => x.Name)
            .ToArrayAsync();
    }

    public async Task<Role?> GetByName(string name)
    {
        using var context = await GetDbContext();

        return await context
            .Roles
            .SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<bool> Save(Role role)
    {
        using var context = await GetDbContext();

        var existing = await context
            .Roles
            .FirstOrDefaultAsync(p => p.Name == role.Name);

        if (existing != null)
        {
            existing.Description = role.Description;
        }
        else
        {
            context.Roles.Add(role);
        }

        var savedRecords = await context.SaveChangesAsync();

        return savedRecords > 0;
    }

    public async Task<int> Delete(string name)
    {
        using var context = await GetDbContext();

        return await context.Roles
            .Where(s => s.Name == name)
            .ExecuteDeleteAsync();
    }
}
