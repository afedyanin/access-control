using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.WebApi.Authorization;
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

    [HttpPost()]
    public async Task<IActionResult> CreateRole(Role role)
    {
        var saved = await _roleRepository.Save(role);

        if (!saved)
        {
            return BadRequest($"Cannot save Role={role}");
        }

        var savedRole = await _roleRepository.GetByName(role.Name);

        if (savedRole == null)
        {
            return NotFound($"Cannot find role by Name={role.Name}");
        }

        return Ok(savedRole);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleRepository.GetAll();
        return Ok(roles ?? []);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetRoleByName(string name)
    {
        var role = await _roleRepository.GetByName(name);

        if (role == null)
        {
            return NotFound($"Cannot find role by Name={name}");
        }

        return Ok(role);
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteRole(string name)
    {
        var deletedCount = await _roleRepository.Delete(name);
        return Ok(deletedCount);
    }
}
