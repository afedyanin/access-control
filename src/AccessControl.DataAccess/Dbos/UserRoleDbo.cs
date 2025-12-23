namespace AccessControl.DataAccess.Dbos;

internal class UserRoleDbo
{
    public string UserName { get; set; } = string.Empty;

    public required UserDbo User { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public required RoleDbo Role { get; set; }
}
