namespace AccessControl.Application.Abstractions;

public interface IResource
{
    public Guid Id { get; }

    public string Name { get; }
}
