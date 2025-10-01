using System.Xml.Linq;
using AccessControl.Contracts;
using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.Model.Repositories;
using AccessControl.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccessControl.WebApi;

[Route("api/permissions")]
[ApiController]

public class PermissionsController : ControllerBase
{
    private readonly IFeatureKeysRepository _featureKeyRepository;
    private readonly IRolesRepository _rolesRepository;
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(
        IFeatureKeysRepository featureKeyRepository,
        IRolesRepository rolesRepository,
        ILogger<PermissionsController> logger)
    {
        _featureKeyRepository = featureKeyRepository;
        _rolesRepository = rolesRepository;
        _logger = logger;
    }

    // TODO: Test it
    [HttpGet("all-featute-keys")]
    public async Task<IActionResult> GetAllFeatureKeysPermissions([FromQuery] string[] roleNames)
    {
        var featureKeys = await _featureKeyRepository.GetAll();

        var res = new Dictionary<string, Permissions>();

        foreach (var featureKey in featureKeys)
        {
            // TODO: Extract method
            var effective = Permissions.None;

            var dict = featureKey.FeatureKeyRoles.ToDictionary(fkr => fkr.RoleName);

            foreach (var roleName in roleNames)
            {
                if (dict.TryGetValue(roleName, out var fkRole))
                {
                    effective |= fkRole.Permissions;
                }
            }

            res[featureKey.Name] = effective;
        }

        return Ok(res);
    }

    [HttpGet("featute-key/{fkName}")]
    public async Task<IActionResult> GetEffectivePermissions(string fkName, [FromQuery] string[] roleNames)
    {
        var featureKey = await _featureKeyRepository.GetByName(fkName);

        if (featureKey == null)
        {
            return NotFound();
        }

        var dict = featureKey.FeatureKeyRoles.ToDictionary(fkr => fkr.RoleName);

        var effective = Permissions.None;

        foreach (var roleName in roleNames)
        {
            if (dict.TryGetValue(roleName, out var fkRole))
            {
                effective |= fkRole.Permissions;
            }
        }

        return Ok(effective);
    }

    [HttpPost("featute-key/{fkName}")]
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

    [HttpPut("featute-key/{fkName}/role/{roleName}/{permissions}")]
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
