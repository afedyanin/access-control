namespace AccessControl.Model.Repositories;
public interface IUsersRepository
{
    public Task<User[]> GetAll();

    public Task<User?> GetByName(string name);

    public Task<bool> Save(UserDbo user);

    public Task<int> Delete(string name);
}
