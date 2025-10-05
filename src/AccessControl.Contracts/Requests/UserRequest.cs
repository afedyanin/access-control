namespace AccessControl.Contracts.Requests;
public class UserRequest
{
    public required string Name { get; set; }

    public string? Email { get; set; }

}
