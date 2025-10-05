using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.Model.Repositories;
using AccessControl.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

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
    public async Task<IActionResult> GetAll()
    {
        var resources = await _resourcesRepository.GetAll();

        return Ok(resources.ToDto());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetId(Guid id)
    {
        var resource = await _resourcesRepository.GetById(id);

        if (resource == null)
        {
            return NotFound();
        }

        return Ok(resource.ToDto());
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] ResourceRequest request)
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
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletedCount = await _resourcesRepository.Delete(id);
        return Ok(deletedCount);
    }
}
