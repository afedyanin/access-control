using AccessControl.Application.Abstractions;

namespace AccessControl.Application.Model;

public class FeatureKey : IResource
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
}
