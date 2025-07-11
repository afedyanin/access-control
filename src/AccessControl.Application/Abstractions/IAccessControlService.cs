using AccessControl.Application.Model;

namespace AccessControl.Application.Abstractions;
public interface IAccessControlService
{
    public Permissions GetPermissions(string resource, string[] roles);
}
