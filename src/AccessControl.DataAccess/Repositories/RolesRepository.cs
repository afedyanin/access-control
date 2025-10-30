using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.DataAccess.Converters;
using AccessControl.DataAccess.Dbos;
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

        var res = await context
            .Roles
            .OrderBy(x => x.Name)
            .ToArrayAsync();

        return res.ToEntity();
    }

    public async Task<Role?> GetByName(string name)
    {
        using var context = await GetDbContext();

        var res = await context
            .Roles
            .SingleOrDefaultAsync(x => x.Name == name);

        return res?.ToEntity();
    }

    public async Task<Role[]> GetByNames(string[] roleNames)
    {
        using var context = await GetDbContext();

        var res = await context
            .Roles
            .Where(r => roleNames.Contains(r.Name))
            .ToArrayAsync();

        return res.ToEntity();
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
            var dbo = new RoleDbo
            {
                Name = role.Name,
                Description = role.Description
            };

            context.Roles.Add(dbo);
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
