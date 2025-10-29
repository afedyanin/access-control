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

    public async Task<bool> Save(UserDbo userDbo)
    {
        using var context = await GetDbContext();

        var rolesToSave = await context
            .Roles
            .Where(r => userDbo.Roles.Contains(r.Name))
            .ToArrayAsync();

        var existingUser = await context
            .Users
            .Include(fk => fk.Roles)
            .FirstOrDefaultAsync(p => p.Name == userDbo.Name);

        if (existingUser == null)
        {
            var newUser = new User
            {
                Name = userDbo.Name,
                Email = userDbo.Email,
                Roles = [.. rolesToSave]
            };

            context.Users.Add(newUser);
        }
        else
        {
            existingUser.Roles.RemoveAll(r => !rolesToSave.Contains(r));
            existingUser.Roles.AddRange(rolesToSave.Where(r => !existingUser.Roles.Contains(r)));
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
