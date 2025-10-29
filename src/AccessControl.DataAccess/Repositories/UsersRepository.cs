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

        var roleNames = user.Roles.Select(x => x.Name).ToArray();

        var newRoles = await context
            .Roles
            .Where(r => roleNames.Contains(r.Name))
            .ToArrayAsync();

        var existing = await context
            .Users
            .Include(fk => fk.Roles)
            .FirstOrDefaultAsync(p => p.Name == user.Name);

        if (existing != null)
        {
            // UPDATE EXISTING USER
            existing.Roles.RemoveAll(r => !newRoles.Contains(r));

            var rolesToAdd = newRoles.Where(r => !existing.Roles.Contains(r)).ToArray();
            existing.Roles.AddRange(rolesToAdd);
        }
        else
        {
            // ADD NEW USER
            user.Roles.Clear();
            user.Roles.AddRange(newRoles);
            context.Users.Add(user);
        }

        //Console.WriteLine("\nTracked changes:");
        //context.ChangeTracker.DetectChanges();
        //Console.WriteLine(context.ChangeTracker.DebugView.LongView);

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
