using AccessControl.Model;
using AccessControl.Model.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess.Repositories;

internal class AccessRolesRepository : RepositoryBase, IAccessRolesRepository
{
    public AccessRolesRepository(IDbContextFactory<AccessControlDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<AccessRole[]> GetAll()
    {
        using var context = await GetDbContext();

        return await context
            .AccesRoles
            .OrderBy(x => x.Name)
            .ToArrayAsync();
    }

    public async Task<AccessRole?> GetById(Guid id)
    {
        using var context = await GetDbContext();

        return await context
            .AccesRoles
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> Save(AccessRole role)
    {
        using var context = await GetDbContext();

        var existing = await context
            .AccesRoles
            .FirstOrDefaultAsync(p => p.Id == role.Id);

        if (existing != null)
        {
            existing.Name = role.Name;
            existing.Description = role.Description;
        }
        else
        {
            context.AccesRoles.Add(role);
        }

        var savedRecords = await context.SaveChangesAsync();

        return savedRecords > 0;
    }

    public async Task<int> Delete(Guid id)
    {
        using var context = await GetDbContext();

        return await context.AccesRoles
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();
    }
}
