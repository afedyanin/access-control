using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

[ApiKey]
[ApiExplorerSettings(GroupName = ApiKeyConsts.ApiGroupName)]
[Route("api/feature-keys")]
[ApiController]

public class FeatureKeysController : ControllerBase
{
    private readonly IFeatureKeysRepository _featureKeyRepository;

    public FeatureKeysController(IFeatureKeysRepository featureKeyRepository)
    {
        _featureKeyRepository = featureKeyRepository;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateFeatureKey(FeatureKey featureKey)
    {
        var saved = await _featureKeyRepository.Save(featureKey);

        if (!saved)
        {
            return BadRequest($"Cannot save FeatureKey={featureKey}");
        }

        var savedFk = await _featureKeyRepository.GetByName(featureKey.Name);

        if (savedFk == null)
        {
            return NotFound($"Cannot find feature key by Name={featureKey.Name}");
        }

        return Ok(savedFk);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllFeatureKeys()
    {
        var featureKeys = await _featureKeyRepository.GetAll();

        return Ok(featureKeys ?? []);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetFeatureKeyByName(string name)
    {
        var featureKey = await _featureKeyRepository.GetByName(name);

        if (featureKey == null)
        {
            return NotFound($"Cannot find feature key by Name={name}");
        }

        return Ok(featureKey);
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteFeatureKey(string name)
    {
        var deletedCount = await _featureKeyRepository.Delete(name);
        return Ok(deletedCount);
    }
}
