using AccessControl.Application.Model;

namespace AccessControl.Application.Abstractions;
public interface IAccessControlService
{
    public Task<Permissions> GetActualPermissions(string featureKey, ICurrentUser currentUser);
}
