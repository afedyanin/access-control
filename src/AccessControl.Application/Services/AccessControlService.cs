using AccessControl.Application.Abstractions;
using AccessControl.Application.Model;

namespace AccessControl.Application.Services;

internal class AccessControlService : IAccessControlService
{
    private PermissionMatrix? _permissionMatrix = null;
    private readonly IPermissionsRepository _permissionsRepository;

    public AccessControlService(IPermissionsRepository permissionsRepository)
    {
        _permissionsRepository = permissionsRepository;
    }

    public async Task<Permissions> GetActualPermissions(string featureKey, ICurrentUser currentUser)
    {
        if (_permissionMatrix == null)
        {
            var entries = await _permissionsRepository.Load();
            _permissionMatrix = new PermissionMatrix(entries);
        }

        return _permissionMatrix == null ?
            Permissions.None :
            _permissionMatrix.GetActualPermissions(featureKey, currentUser.Roles);
    }

    public void Reset()
    {
        _permissionMatrix = null;
    }
}
