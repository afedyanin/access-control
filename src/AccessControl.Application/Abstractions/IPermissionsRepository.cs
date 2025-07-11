using AccessControl.Application.Model;

namespace AccessControl.Application.Abstractions;

public interface IPermissionsRepository
{
    public Task<PermissionsEntry[]> Load();

    public Task Save(PermissionsEntry[] entries);
}
