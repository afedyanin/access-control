using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

[Route("api/feature-keys")]
[ApiController]

public class FeatureKeysController : ControllerBase
{
    private readonly IFeatureKeyRepository _featureKeyRepository;
    public FeatureKeysController(IFeatureKeyRepository featureKeyRepository)
    {
        _featureKeyRepository = featureKeyRepository;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var featureKeys = await _featureKeyRepository.GetAll();

        // TODO: convert to DTO: hide collections & use RolePermissions
        return Ok(featureKeys);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var fk = await _featureKeyRepository.GetById(id);

        if (fk == null)
        {
            return NotFound();
        }

        // TODO: convert to DTO: hide collections & use RolePermissions
        return Ok(fk);
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] FeatureKeyRequest request)
    {
        var role = new FeatureKey
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        var saved = await _featureKeyRepository.Save(role);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok(role.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FeatureKeyRequest request)
    {
        var found = await _featureKeyRepository.GetById(id);

        if (found == null)
        {
            return NotFound();
        }

        found.Name = request.Name;

        var saved = await _featureKeyRepository.Save(found);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletedCount = await _featureKeyRepository.Delete(id);
        return Ok();
    }
}
