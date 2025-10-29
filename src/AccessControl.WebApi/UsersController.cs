using AccessControl.Contracts;
using AccessControl.Model.Repositories;
using AccessControl.WebApi.Authorization;
using AccessControl.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.WebApi;

[ApiKey]
[ApiExplorerSettings(GroupName = ApiKeyConsts.ApiGroupName)]
[Route("api/users")]
[ApiController]

public class UsersController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;

    public UsersController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateUser(UserDto user)
    {
        var saved = await _usersRepository.Save(user.FromDto());

        if (!saved)
        {
            return BadRequest($"Cannot save User={user}");
        }

        var savedUser = await _usersRepository.GetByName(user.Name);

        if (savedUser == null)
        {
            return NotFound($"Cannot find user by Name={user.Name}");
        }

        return Ok(savedUser.ToDto());
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _usersRepository.GetAll();

        return Ok(users.ToDto());
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var user = await _usersRepository.GetByName(name);

        if (user == null)
        {
            return NotFound($"Cannot find user by Name={name}");
        }

        return Ok(user.ToDto());
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteUser(string name)
    {
        var deletedCount = await _usersRepository.Delete(name);
        return Ok(deletedCount);
    }
}
