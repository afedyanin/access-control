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

    [HttpGet("featute-key/{fkey}")]
    public async Task<IActionResult> GetEffectivePermissions(string fkey, string[] roles)
    {
        // get fk by name
        // get roles by names
        // match roles with fk collection FeatureKeyAccessRoles
        return Ok(Permissions.None);
    }

    [HttpPut("{fkey}")]
    public async Task<IActionResult> UpdatePermissions(string fkey, [FromBody] PermissionsRequest request)
    {
        //var found = await _featureKeyRepository.GetByName(id);
/*
        if (found == null)
        {
            return NotFound();
        }
*/
        // TODO: Update permissions here

        return Ok();
    }

}
