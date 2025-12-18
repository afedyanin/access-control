using AccessControl.AdminUI.Models;
using Microsoft.AspNetCore.Components;

namespace AccessControl.AdminUI.Components.Common;

public partial class FeatureKeysDialog
{
    [Parameter]
    public FeatureKeyRolePermissionsModel Content { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        Content.RoleName = Content.AllRoles[0];
        await base.OnInitializedAsync();
    }
}
