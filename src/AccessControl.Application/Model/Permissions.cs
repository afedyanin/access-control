namespace AccessControl.Application.Model;

[Flags]
public enum Permissions
{
    None = 0,
    Read = 1,
    Write = 2,
    ReadWrite = Read | Write,
    Delete = 4,
    Execute = 8,
    Full = Read | Write | Delete | Execute,
}
