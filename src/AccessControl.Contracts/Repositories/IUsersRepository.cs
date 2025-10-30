using AccessControl.Contracts.Entities;

namespace AccessControl.Contracts.Repositories;
public interface IUsersRepository
{
    public Task<User[]> GetAll();

    public Task<User?> GetByName(string name);

    public Task<bool> Save(User user);

    public Task<int> Delete(string name);
}
