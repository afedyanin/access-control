using AccessControl.Contracts;
using AccessControl.Contracts.Requests;
using AccessControl.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

[Route("api/permissions")]
[ApiController]

public class PermissionsController : ControllerBase
{
    private readonly IFeatureKeysRepository _featureKeyRepository;
    public PermissionsController(IFeatureKeysRepository featureKeyRepository)
    {
        _featureKeyRepository = featureKeyRepository;
    }

    [HttpGet("featute-key/{fkName}")]
    public async Task<IActionResult> GetEffectivePermissions(string fkName, string[] roleNames)
    {
        var found = await _featureKeyRepository.GetByName(fkName);

        if (found == null)
        {
            return NotFound();
        }

        // get roles by names
        // match roles with fk collection FeatureKeyAccessRoles
        return Ok(Permissions.None);
    }

    [HttpPut("featute-key/{fkName}")]
    public async Task<IActionResult> UpdatePermissions(string fkName, [FromBody] PermissionsRequest request)
    {
        var found = await _featureKeyRepository.GetByName(fkName);

        if (found == null)
        {
            return NotFound();
        }

        // TODO: Update permissions here

        return Ok();
    }

}
