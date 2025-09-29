namespace AccessControl.Contracts.Requests;

public class RoleRequest
{
    public required string Name { get; set; }

    public string? Description { get; set; }
}
