using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

[Route("api/roles")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IAccessRoleRepository _roleRepository;
    public RolesController(IAccessRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleRepository.GetAll();
        return Ok(roles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var role = await _roleRepository.GetById(id);

        if (role == null)
        {
            return NotFound();
        }

        return Ok(role);
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] RoleRequest request)
    {
        var role = new AccessRole
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
        };

        var saved = await _roleRepository.Save(role);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok(role.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RoleRequest request)
    {
        var found = await _roleRepository.GetById(id);

        if (found == null)
        {
            return NotFound();
        }

        found.Name = request.Name;
        found.Description = request.Description;

        var saved = await _roleRepository.Save(found);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok();
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletedCount = await _roleRepository.Delete(id);
        return Ok();
    }
}
