using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.Model.Repositories;
using AccessControl.WebApi.Authorization;
using AccessControl.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

[ApiKey]
[ApiExplorerSettings(GroupName = ApiKeyConsts.ApiGroupName)]
[Route("api/roles")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRolesRepository _roleRepository;
    public RolesController(IRolesRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleRepository.GetAll();
        return Ok(roles.ToDto());
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetRoleByName(string name)
    {
        var role = await _roleRepository.GetByName(name);

        if (role == null)
        {
            return NotFound();
        }

        return Ok(role.ToDto());
    }

    [HttpPost()]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
    {
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description,
        };

        var saved = await _roleRepository.Save(role);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok(role.ToDto());
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateRole([FromBody] RoleRequest request)
    {
        var found = await _roleRepository.GetByName(request.Name);

        if (found == null)
        {
            return NotFound();
        }

        found.Description = request.Description;

        var saved = await _roleRepository.Save(found);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok(found.ToDto());
    }


    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteRole(string name)
    {
        var deletedCount = await _roleRepository.Delete(name);
        return Ok(deletedCount);
    }
}
