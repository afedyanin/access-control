namespace AccessControl.DataAccess.Dbos;
internal class UserDbo
{
    public required string Name { get; set; }

    public string? Email { get; set; }

    public List<RoleDbo> Roles { get; set; } = [];

    public List<UserRoleDbo> UserRoles { get; set; } = [];
}
