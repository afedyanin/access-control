using AccessControl.AdminUI.Components.Common;
using AccessControl.AdminUI.Models;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AccessControl.AdminUI.Components.Pages;

public partial class FeatureKeys
{
    private static List<string> _allFeatureKeys =
    [
        "FK 001",
        "FK 002",
        "FK 003",
        "FK 004",
    ];

    private static List<string> _allRoles =
    [
        "Role 001",
        "Role 002",
        "Role 003",
        "Role 004",
    ];


    List<FeatureKeyRolePermissionsModel> RowsGrid = new()
    {
        new FeatureKeyRolePermissionsModel
        {
            FeatureKey = "Feature Key001",
            RoleName = "Some Role 01",
            PermissionRead = true,
            PermissionWrite = true,
        },

        new FeatureKeyRolePermissionsModel
        {
            FeatureKey = "Feature Key001",
            RoleName = "Some Role 02",
            PermissionRead = true,
            PermissionWrite = false,
        },

        new FeatureKeyRolePermissionsModel
        {
            FeatureKey = "Feature Key002",
            RoleName = "Some Role 01",
            PermissionRead = true,
            PermissionWrite = true,
            PermissionExecute = true,
            PermissionDelete = true,
        },

        new FeatureKeyRolePermissionsModel
        {
            FeatureKey = "Feature Key002",
            RoleName = "Some Role 03",
            PermissionRead = true,
            PermissionExecute = true,
        },
    };


    private FeatureKeyRolePermissionsModel _initialModel = new FeatureKeyRolePermissionsModel
    {
        FeatureKey = "",
        RoleName = "",
        AllFeatureKeys = _allFeatureKeys,
        AllRoles = _allRoles,
    };

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

        var dialog = await DialogService.ShowDialogAsync<FeatureKeysDialog>(_initialModel, parameters);
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
            RowsGrid.Add(new FeatureKeyRolePermissionsModel
            {
                FeatureKey = model.FeatureKey,
                RoleName = role
            });

            saved = true;
        }

        if (_allFeatureKeys.FirstOrDefault(fk => fk == model.FeatureKey) == null)
        {
            _allFeatureKeys.Add(model.FeatureKey);
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

        var found = RowsGrid.FirstOrDefault(r => r.FeatureKey == model.FeatureKey && r.RoleName == model.RoleName);

        if (found != null)
        {
            RowsGrid.Remove(found);
            ToastService.ShowWarning($"{model.FeatureKey}:{model.RoleName} deleted!");
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

        ToastService.ShowSuccess("All cahnges saved!");
    }


    private void HandleRowClick(FluentDataGridRow<FeatureKeyRolePermissionsModel> row)
    {
        // DemoLogger.WriteLine($"Row clicked: {row.RowIndex}");
    }

    private void HandleRowFocus(FluentDataGridRow<FeatureKeyRolePermissionsModel> row)
    {
        // DemoLogger.WriteLine($"Row focused: {row.RowIndex}");
    }

    private void HandleCellClick(FluentDataGridCell<FeatureKeyRolePermissionsModel> cell)
    {
        // DemoLogger.WriteLine($"Cell clicked: {cell.GridColumn}");
    }

    private void HandleCellFocus(FluentDataGridCell<FeatureKeyRolePermissionsModel> cell)
    {
        // DemoLogger.WriteLine($"Cell focused : {cell.GridColumn}");
    }
}
