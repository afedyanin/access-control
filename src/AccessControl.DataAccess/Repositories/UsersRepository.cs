using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.DataAccess.Converters;
using AccessControl.DataAccess.Dbos;
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

        var res = await context
            .Users
            .Include(fk => fk.Roles)
            .OrderBy(x => x.Name)
            .ToArrayAsync();

        return res.ToEntity();
    }

    public async Task<User?> GetByName(string name)
    {
        using var context = await GetDbContext();

        var res = await context
            .Users
            .Include(fk => fk.Roles)
            .SingleOrDefaultAsync(x => x.Name == name);

        return res?.ToEntity();
    }

    public async Task<bool> Save(User user)
    {
        using var context = await GetDbContext();

        var rolesToSave = await context
            .Roles
            .Where(r => user.Roles.Contains(r.Name))
            .ToArrayAsync();

        var existingUser = await context
            .Users
            .Include(fk => fk.Roles)
            .FirstOrDefaultAsync(p => p.Name == user.Name);

        if (existingUser == null)
        {
            var newUser = new UserDbo
            {
                Name = user.Name,
                Email = user.Email,
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
