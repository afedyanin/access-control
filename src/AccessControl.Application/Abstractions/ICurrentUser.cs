namespace AccessControl.Application.Abstractions;

public interface ICurrentUser
{
    public Guid Id { get; }

    public string Name { get; }

    public string[] Roles { get; }
}
