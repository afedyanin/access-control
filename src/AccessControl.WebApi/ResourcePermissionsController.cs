using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.WebApi.Authorization;
using AccessControl.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccessControl.WebApi;

[ApiKey]
[ApiExplorerSettings(GroupName = ApiKeyConsts.ApiGroupName)]
[Route("api/resource-permissions")]
[ApiController]

public class ResourcePermissionsController : ControllerBase
{
    private readonly IResourcesRepository _resourcesRepository;
    private readonly IRolesRepository _rolesRepository;
    private readonly ILogger<ResourcePermissionsController> _logger;

    public ResourcePermissionsController(
        IResourcesRepository resourcesRepository,
        IRolesRepository rolesRepository,
        ILogger<ResourcePermissionsController> logger)
    {
        _resourcesRepository = resourcesRepository;
        _rolesRepository = rolesRepository;
        _logger = logger;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllResourcesPermissions([FromQuery] string[] roleNames)
    {
        var featureKeys = await _resourcesRepository.GetAll();
        var res = new Dictionary<string, Permissions>();

        foreach (var featureKey in featureKeys)
        {
            var effective = featureKey.GetEffectivePermissions(roleNames);
            res[featureKey.Name] = effective;
        }

        return Ok(res);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEffectiveResourcePermissions(Guid id, [FromQuery] string[] roleNames)
    {
        var resource = await _resourcesRepository.GetById(id);

        if (resource == null)
        {
            return NotFound();
        }

        var permissions = resource.GetEffectivePermissions(roleNames);

        return Ok(permissions);
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> CreateResourcePermissions(Guid id, [FromBody] PermissionsRequest request)
    {
        var resource = await _resourcesRepository.GetById(id);

        if (resource == null)
        {
            return NotFound();
        }

        var dict = request.Permissions.ToDictionary(rp => rp.RoleName);
        var roles = await _rolesRepository.GetByNames([.. dict.Keys]);

        resource.ResourceRoles.Clear();

        foreach (var role in roles)
        {
            var fkr = new ResourceRole
            {
                Resource = resource,
                Role = role,
                Permissions = dict[role.Name].Permissions,
            };

            resource.ResourceRoles.Add(fkr);
        }

        var saved = await _resourcesRepository.Save(resource);

        if (!saved)
        {
            return BadRequest($"Cannot save permissions for ResourceId={id}");
        }

        return Ok(resource.ToDto());
    }

    [HttpPut("{id:guid}/role/{roleName}/{permissions}")]
    public async Task<IActionResult> UpdateResourcePermissions(Guid id, string roleName, Permissions permissions)
    {
        var resource = await _resourcesRepository.GetById(id);

        if (resource == null)
        {
            _logger.LogError($"Resource with Id={id} is not found.");
            return NotFound();
        }

        var role = await _rolesRepository.GetByName(roleName);

        if (role == null)
        {
            _logger.LogError($"Role with name={roleName} is not found.");
            return NotFound();
        }

        var fkRole = resource.ResourceRoles.FirstOrDefault(fkr => fkr.RoleName == role.Name);

        if (fkRole == null)
        {
            fkRole = new ResourceRole
            {
                Resource = resource,
                Role = role,
                Permissions = permissions,
            };

            resource.ResourceRoles.Add(fkRole);
        }
        else
        {
            fkRole.Permissions = permissions;
        }

        var saved = await _resourcesRepository.Save(resource);

        if (!saved)
        {
            var message = $"Cannot update permissions for ResourceId={id} and Role={roleName}";
            _logger.LogError(message);
            return BadRequest(message);
        }

        return Ok(resource.ToDto());
    }
}
