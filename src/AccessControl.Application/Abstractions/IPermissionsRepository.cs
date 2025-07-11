using AccessControl.Application.Model;

namespace AccessControl.Application.Abstractions;

public interface IPermissionsRepository
{
    public PermissionsEntry[] GetAll();

    public void Save(PermissionsEntry[] entries);
}
