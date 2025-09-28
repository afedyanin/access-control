using AccessControl.Model;

namespace AccessControl.Application.Abstractions;
public interface IAccessControlService
{
    public AccessPermissions GetPermissions(string resource, string[] roles);
}
