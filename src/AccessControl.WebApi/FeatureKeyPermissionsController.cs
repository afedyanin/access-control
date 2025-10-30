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
[Route("api/feature-key-permissions")]
[ApiController]

public class FeatureKeyPermissionsController : ControllerBase
{
    private readonly IFeatureKeysRepository _featureKeyRepository;
    private readonly IRolesRepository _rolesRepository;
    private readonly ILogger<FeatureKeyPermissionsController> _logger;

    public FeatureKeyPermissionsController(
        IFeatureKeysRepository featureKeyRepository,
        IRolesRepository rolesRepository,
        ILogger<FeatureKeyPermissionsController> logger)
    {
        _featureKeyRepository = featureKeyRepository;
        _rolesRepository = rolesRepository;
        _logger = logger;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllFeatureKeysPermissions([FromQuery] string[] roleNames)
    {
        var featureKeys = await _featureKeyRepository.GetAll();
        var res = new Dictionary<string, Permissions>();

        foreach (var featureKey in featureKeys)
        {
            var effective = featureKey.GetEffectivePermissions(roleNames);
            res[featureKey.Name] = effective;
        }

        return Ok(res);
    }

    [HttpGet("{fkName}")]
    public async Task<IActionResult> GetEffectivePermissions(string fkName, [FromQuery] string[] roleNames)
    {
        var featureKey = await _featureKeyRepository.GetByName(fkName);

        if (featureKey == null)
        {
            return NotFound();
        }

        var permissions = featureKey.GetEffectivePermissions(roleNames);

        return Ok(permissions);
    }

    [HttpPost("{fkName}")]
    public async Task<IActionResult> CreatePermissions(string fkName, [FromBody] PermissionsRequest request)
    {
        var featureKey = await _featureKeyRepository.GetByName(fkName);

        if (featureKey == null)
        {
            return NotFound();
        }

        var dict = request.Permissions.ToDictionary(rp => rp.RoleName);
        var roles = await _rolesRepository.GetByNames([.. dict.Keys]);

        featureKey.FeatureKeyRoles.Clear();

        foreach (var role in roles)
        {
            var fkr = new FeatureKeyRole
            {
                FeatureKey = featureKey,
                Role = role,
                Permissions = dict[role.Name].Permissions,
            };

            featureKey.FeatureKeyRoles.Add(fkr);
        }

        var saved = await _featureKeyRepository.Save(featureKey);

        if (!saved)
        {
            return BadRequest($"Cannot save permissions for FeatureKey={fkName}");
        }

        return Ok(featureKey.ToDto());
    }

    [HttpPut("{fkName}/role/{roleName}/{permissions}")]
    public async Task<IActionResult> UpdatePermissions(string fkName, string roleName, Permissions permissions)
    {
        var featureKey = await _featureKeyRepository.GetByName(fkName);

        if (featureKey == null)
        {
            _logger.LogError($"Feature key with name={fkName} is not found.");
            return NotFound();
        }

        var role = await _rolesRepository.GetByName(roleName);

        if (role == null)
        {
            _logger.LogError($"Role with name={roleName} is not found.");
            return NotFound();
        }

        var fkRole = featureKey.FeatureKeyRoles.FirstOrDefault(fkr => fkr.RoleName == role.Name);

        if (fkRole == null)
        {
            fkRole = new FeatureKeyRole
            {
                FeatureKey = featureKey,
                Role = role,
                Permissions = permissions,
            };

            featureKey.FeatureKeyRoles.Add(fkRole);
        }
        else
        {
            fkRole.Permissions = permissions;
        }

        var saved = await _featureKeyRepository.Save(featureKey);

        if (!saved)
        {
            var message = $"Cannot update permissions for FeatureKey={fkName} and Role={roleName}";
            _logger.LogError(message);
            return BadRequest(message);
        }

        return Ok(featureKey.ToDto());
    }
}
