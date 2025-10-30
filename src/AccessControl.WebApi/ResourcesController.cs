using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Repositories;
using AccessControl.WebApi.Authorization;
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

    [HttpPost()]
    public async Task<IActionResult> CreateResource(Resource resource)
    {
        var saved = await _resourcesRepository.Save(resource);

        if (!saved)
        {
            return BadRequest($"Cannot save Resource={resource}");
        }

        var savedRes = await _resourcesRepository.GetById(resource.Id);

        if (savedRes == null)
        {
            return NotFound($"Cannot find resource by Id={resource.Id}");
        }

        return Ok(savedRes);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllResources()
    {
        var resources = await _resourcesRepository.GetAll();

        return Ok(resources ?? []);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetResourceById(Guid id)
    {
        var resource = await _resourcesRepository.GetById(id);

        if (resource == null)
        {
            return NotFound($"Cannot find resource key by Id={id}");
        }

        return Ok(resource);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteResource(Guid id)
    {
        var deletedCount = await _resourcesRepository.Delete(id);
        return Ok(deletedCount);
    }
}
