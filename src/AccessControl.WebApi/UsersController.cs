using AccessControl.Contracts.Requests;
using AccessControl.Model;
using AccessControl.Model.Repositories;
using AccessControl.WebApi.Authorization;
using AccessControl.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccessControl.WebApi;

[ApiKey]
[Route("api/users")]
[ApiController]

public class UsersController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;
    private readonly IRolesRepository _rolesRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IUsersRepository usersRepository,
        IRolesRepository rolesRepository,
        ILogger<UsersController> logger)
    {
        _usersRepository = usersRepository;
        _rolesRepository = rolesRepository;
        _logger = logger;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var users = await _usersRepository.GetAll();

        return Ok(users.ToDto());
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var role = await _usersRepository.GetByName(name);

        if (role == null)
        {
            return NotFound();
        }

        return Ok(role.ToDto());
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] UserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
        };

        var saved = await _usersRepository.Save(user);

        if (!saved)
        {
            return BadRequest();
        }

        return Ok(user.ToDto());
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        var deletedCount = await _usersRepository.Delete(name);
        return Ok(deletedCount);
    }

    [HttpPost("{name}/roles")]
    public async Task<IActionResult> AssignRoles(string name, [FromQuery] string[] roleNames)
    {
        var user = await _usersRepository.GetByName(name);

        if (user == null)
        {
            return NotFound();
        }

        var roles = await _rolesRepository.GetByNames(roleNames ?? []);

        user.UserRoles.Clear();

        foreach (var role in roles)
        {
            var ur = new UserRole
            {
                User = user,
                Role = role,
            };

            user.UserRoles.Add(ur);
        }

        var saved = await _usersRepository.Save(user);

        if (!saved)
        {
            return BadRequest($"Cannot save roles for User={name}");
        }

        return Ok(user.ToDto());
    }

    [HttpPut("{name}/roles/{roleName}")]
    public async Task<IActionResult> AssignRole(string name, string roleName)
    {
        var user = await _usersRepository.GetByName(name);

        if (user == null)
        {
            _logger.LogError($"User with name={name} is not found.");
            return NotFound();
        }

        var role = await _rolesRepository.GetByName(roleName);

        if (role == null)
        {
            _logger.LogError($"Role with name={roleName} is not found.");
            return NotFound();
        }

        var userRole = user.UserRoles.FirstOrDefault(ur => ur.RoleName == role.Name);

        if (userRole != null)
        {
            // Role already assigned
            return Ok(user.ToDto());
        }

        userRole = new UserRole
        {
            User = user,
            Role = role,
        };

        user.UserRoles.Add(userRole);

        var saved = await _usersRepository.Save(user);

        if (!saved)
        {
            var message = $"Cannot update role for User={name} Role={roleName}";
            _logger.LogError(message);
            return BadRequest(message);
        }

        return Ok(user.ToDto());
    }
}
