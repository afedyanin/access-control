using AccessControl.Model;
using AccessControl.Model.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess.Repositories;

internal class UsersRepository : RepositoryBase, IUsersRepository
{
    public UsersRepository(IDbContextFactory<AccessControlDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<User[]> GetAll()
    {
        using var context = await GetDbContext();

        return await context
            .Users
            .Include(fk => fk.Roles)
            .OrderBy(x => x.Name)
            .ToArrayAsync();
    }

    public async Task<User?> GetByName(string name)
    {
        using var context = await GetDbContext();

        return await context
            .Users
            .Include(fk => fk.Roles)
            .SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<bool> Save(User user)
    {
        using var context = await GetDbContext();

        var existing = await context
            .Users
            .FirstOrDefaultAsync(p => p.Name == user.Name);

        if (existing != null)
        {
            existing.Roles.Clear();
            foreach (var role in user.Roles)
            {
                context.Attach(role);
                existing.Roles.Add(role);
            }

            existing.UserRoles.Clear();
            foreach (var userRole in user.UserRoles)
            {
                context.Attach(userRole.Role);
                existing.UserRoles.Add(userRole);
            }
        }
        else
        {
            context.Users.Add(user);
        }

        var savedRecords = await context.SaveChangesAsync();

        return savedRecords > 0;
    }

    public async Task<int> Delete(string name)
    {
        using var context = await GetDbContext();

        return await context.Users
            .Where(s => s.Name == name)
            .ExecuteDeleteAsync();
    }
}
