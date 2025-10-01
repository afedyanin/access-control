using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.Model.Repositories;
using AccessControl.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

[Route("api/feature-keys")]
[ApiController]

public class FeatureKeysController : ControllerBase
{
    private readonly IFeatureKeysRepository _featureKeyRepository;
    public FeatureKeysController(IFeatureKeysRepository featureKeyRepository)
    {
        _featureKeyRepository = featureKeyRepository;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var featureKeys = await _featureKeyRepository.GetAll();

        return Ok(featureKeys.ToDto());
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var featureKey = await _featureKeyRepository.GetByName(name);

        if (featureKey == null)
        {
            return NotFound();
        }

        return Ok(featureKey.ToDto());
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] FeatureKeyRequest request)
    {
        var featureKey = new FeatureKey
        {
            Name = request.Name,
        };

        var saved = await _featureKeyRepository.Save(featureKey);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok(featureKey.ToDto());
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        var deletedCount = await _featureKeyRepository.Delete(name);
        return Ok(deletedCount);
    }
}
