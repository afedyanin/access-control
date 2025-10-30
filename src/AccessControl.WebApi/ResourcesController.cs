using AccessControl.Contracts.Repositories;
using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.WebApi.Authorization;
using AccessControl.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

[ApiKey]
[ApiExplorerSettings(GroupName = ApiKeyConsts.ApiGroupName)]
[Route("api/resources")]
[ApiController]

public class ResourcesController : ControllerBase
{
    private readonly IResourcesRepository _resourcesRepository;

    public ResourcesController(IResourcesRepository resourcesRepository)
    {
        _resourcesRepository = resourcesRepository;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllResources()
    {
        var resources = await _resourcesRepository.GetAll();

        return Ok(resources.ToDto());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetResourceById(Guid id)
    {
        var resource = await _resourcesRepository.GetById(id);

        if (resource == null)
        {
            return NotFound();
        }

        return Ok(resource.ToDto());
    }

    [HttpPost()]
    public async Task<IActionResult> CreateResource([FromBody] ResourceRequest request)
    {
        // TODO: Check if already exists

        var resource = new Resource
        {
            Id = request.Id,
            Name = request.Name,
        };

        var saved = await _resourcesRepository.Save(resource);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok(resource.ToDto());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteResource(Guid id)
    {
        var deletedCount = await _resourcesRepository.Delete(id);
        return Ok(deletedCount);
    }
}
