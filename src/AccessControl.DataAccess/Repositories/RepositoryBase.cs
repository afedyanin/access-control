using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess.Repositories;

internal abstract class RepositoryBase
{
    private readonly IDbContextFactory<AccessControlDbContext> _contextFactory;

    protected Task<AccessControlDbContext> GetDbContext() => _contextFactory.CreateDbContextAsync();

    protected RepositoryBase(IDbContextFactory<AccessControlDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
}
