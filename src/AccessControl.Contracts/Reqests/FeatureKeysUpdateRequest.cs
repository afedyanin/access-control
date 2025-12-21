using AccessControl.Contracts.Entities;

namespace AccessControl.Contracts.Reqests;

public record class FeatureKeysUpdateRequest
{
    public FeatureKey[] ChangedKeys { get; set; } = [];

    public string[] DeletedKeys { get; set; } = [];
}
