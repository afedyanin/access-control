using AccessControl.AdminUI.Components.Common;
using AccessControl.AdminUI.Models;
using AccessControl.Contracts;
using AccessControl.Contracts.Entities;
using AccessControl.Contracts.Reqests;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AccessControl.AdminUI.Components.Pages;

public partial class FeatureKeys
{
    private List<string> _allFeatureKeyNames = [];
    private string[] _allRoles = [];
    private List<FeatureKeyRolePermissionsModel> _rowsGrid = [];
    private FeatureKeyRolePermissionsModel? _initialModel;

    private FeatureKeyChangeTracker? _changeTracker;

    [Inject]
    private IAccessControlClient ApiClient { get; set; } = default!;

    [Inject]
    private ILogger<FeatureKeys> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await InitModel();
        await base.OnInitializedAsync();
    }

    private async Task InitModel()
    {
        var roles = await ApiClient.GetAllRoles();
        _allRoles = [.. roles.Select(r => r.Name)];

        var allKeys = await ApiClient.GetAllFeatureKeys();
        _allFeatureKeyNames = [.. allKeys.Select(x => x.Name)];
        _rowsGrid = ToModel(allKeys);
        _changeTracker = new FeatureKeyChangeTracker(allKeys);
        _initialModel = new FeatureKeyRolePermissionsModel
        {
            FeatureKey = "",
            RoleName = "",
            AllFeatureKeys = _allFeatureKeyNames,
            AllRoles = _allRoles,
        };
    }

    private async Task OpenDialogAsync()
    {
        var parameters = new DialogParameters()
        {
            Alignment = HorizontalAlignment.Center,
            Title = "Create new feature key permissions",
            PrimaryAction = "Save",
            SecondaryAction = "Cancel",
            Width = "500px",
            TrapFocus = true,
            Modal = true,
            PreventScroll = true
        };

        var dialog = await DialogService.ShowDialogAsync<FeatureKeysDialog>(_initialModel!, parameters);
        var result = await dialog.Result;

        if (result.Cancelled)
        {
            return;
        }

        if (result.Data is null)
        {
            return;
        }

        if (result.Data is not FeatureKeyRolePermissionsModel model)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(model.FeatureKey))
        {
            return;
        }

        var saved = false;

        foreach (var role in model.SelectedRoles ?? [])
        {
            if (_changeTracker!.TryAdd(model.FeatureKey, role, Permissions.None))
            {
                _rowsGrid.Add(new FeatureKeyRolePermissionsModel
                {
                    FeatureKey = model.FeatureKey,
                    RoleName = role
                });

                saved = true;
            }
        }

        if (_allFeatureKeyNames.FirstOrDefault(fk => fk == model.FeatureKey) == null)
        {
            _allFeatureKeyNames.Add(model.FeatureKey);
        }

        if (saved)
        {
            var roles = string.Join(',', model.SelectedRoles ?? []);
            ToastService.ShowSuccess($"FK={model.FeatureKey} and Role(s)={roles} selected.");
        }

        /*
        Navigation.NavigateTo($"backtests/details/{model.Backtest.Id}");
        */
    }

    private async Task DiscardChangesAsync()
    {
        var confirmation = await DialogService.ShowConfirmationAsync(
            $"Discard all changes?",
            "Yes",
            "No",
            $"All cahnges will be lost!");

        var result = await confirmation.Result;

        if (result.Cancelled)
        {
            return;
        }

        await InitModel();
        ToastService.ShowSuccess("All changes discarded!");
    }

    private async Task HandleDeleteAction(FeatureKeyRolePermissionsModel model)
    {
        var confirmation = await DialogService.ShowConfirmationAsync(
            $"Delete {model.FeatureKey}:{model.RoleName}?",
            "Yes",
            "No",
            $"Deleting {model.FeatureKey}:{model.RoleName}");

        var result = await confirmation.Result;

        if (result.Cancelled)
        {
            return;
        }

        var found = _rowsGrid.FirstOrDefault(r => r.FeatureKey == model.FeatureKey && r.RoleName == model.RoleName);

        if (found != null)
        {
            if (_changeTracker!.TryDelete(found.FeatureKey, found.RoleName))
            {
                _rowsGrid.Remove(found);
                ToastService.ShowWarning($"{model.FeatureKey}:{model.RoleName} deleted!");
            }
        }
    }

    private async Task SubmitChangesAsync()
    {
        var confirmation = await DialogService.ShowConfirmationAsync(
            $"Save all changes?",
            "Yes",
            "No",
            $"Saving permissions");

        var result = await confirmation.Result;

        if (result.Cancelled)
        {
            return;
        }

        foreach (var model in _rowsGrid)
        {
            _changeTracker!.TryUpdate(model.FeatureKey, model.RoleName, model.GetPermissions());
        }

        var request = new FeatureKeysUpdateRequest
        {
            ChangedKeys = [.. _changeTracker!.GetChangedKeys()],
            DeletedKeys = [.. _changeTracker!.GetDeletedKeys()],
        };

        await ApiClient.Update(request);
        await InitModel();

        ToastService.ShowSuccess("All cahnges saved!");
    }

    private static List<FeatureKeyRolePermissionsModel> ToModel(FeatureKey[] keys)
    {
        var res = keys.SelectMany(key => key.RolePermissions,
            (key, role) =>
             new FeatureKeyRolePermissionsModel
             {
                 FeatureKey = key.Name,
                 RoleName = role.RoleName,
                 PermissionRead = role.Permissions.HasFlag(Permissions.Read),
                 PermissionWrite = role.Permissions.HasFlag(Permissions.Write),
                 PermissionExecute = role.Permissions.HasFlag(Permissions.Execute),
                 PermissionDelete = role.Permissions.HasFlag(Permissions.Delete),
             });

        return [.. res.OrderBy(t => t.FeatureKey).ThenBy(t => t.RoleName)];
    }
}
