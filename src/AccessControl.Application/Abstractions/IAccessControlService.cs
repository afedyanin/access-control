using AccessControl.Contracts;

namespace AccessControl.Application.Abstractions;
public interface IAccessControlService
{
    public Permissions GetPermissions(string resource, string[] roles);
}
