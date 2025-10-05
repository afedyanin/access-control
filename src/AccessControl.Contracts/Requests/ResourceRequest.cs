namespace AccessControl.Contracts.Requests;

public class ResourceRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
