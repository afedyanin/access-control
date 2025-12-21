using AccessControl.Contracts.Entities;

namespace AccessControl.AdminUI.Models;

public class FeatureKeyChangeTracker
{
    private Dictionary<string, Dictionary<string, Permissions>> _keysDict = [];

    private readonly HashSet<string> _changedKeys = [];
    private readonly HashSet<string> _deletedKeys = [];

    public FeatureKeyChangeTracker(FeatureKey[] allKeys)
    {
        Reset(allKeys);
    }

    public void Reset(FeatureKey[] allKeys)
    {
        _keysDict = CreateStructuredPermissions(allKeys);
    }

    public bool TryAdd(string featureKey, string roleName, Permissions permissions)
    {
        _keysDict.TryGetValue(featureKey, out var rolePermissions);

        if (rolePermissions == null)
        {
            rolePermissions = new Dictionary<string, Permissions>
            {
                [roleName] = permissions
            };

            // Add new FK with RolePermissions dict
            _keysDict[featureKey] = rolePermissions;
            _changedKeys.Add(featureKey);
            return true;
        }

        // Feature key already exists. Check role entry
        if (rolePermissions.TryAdd(roleName, permissions))
        {
            _changedKeys.Add(featureKey);
            return true;
        }

        return false;
    }

    public bool TryUpdate(string featureKey, string roleName, Permissions permissions)
    {
        _keysDict.TryGetValue(featureKey, out var rolePermissions);

        if (rolePermissions == null)
        {
            return false;
        }

        if (!rolePermissions.ContainsKey(roleName))
        {
            return false;
        }

        rolePermissions[roleName] = permissions;
        _changedKeys.Add(featureKey);
        return true;
    }

    public bool TryDelete(string featureKey, string roleName)
    {
        _keysDict.TryGetValue(featureKey, out var rolePermissions);

        if (rolePermissions == null)
        {
            return false;
        }

        if (!rolePermissions.ContainsKey(roleName))
        {
            return false;
        }

        rolePermissions.Remove(roleName);

        if (rolePermissions.Count == 0)
        {
            _keysDict.Remove(featureKey);
            _deletedKeys.Add(featureKey);
        }
        else
        {
            _changedKeys.Add(featureKey);
        }

        return true;
    }

    // TODO: call update API
    public IEnumerable<FeatureKey> GetChangedKeys()
    {
        var res = new List<FeatureKey>();

        foreach (var keyName in _changedKeys)
        {
            _keysDict.TryGetValue(keyName, out var rolePermissionsDict);

            if (rolePermissionsDict != null)
            {
                var fk = new FeatureKey
                {
                    Name = keyName,
                    RolePermissions = [.. rolePermissionsDict
                    .Select(kvp => new RolePermissions
                    {
                        RoleName = kvp.Key,
                        Permissions = kvp.Value
                    })]
                };

                res.Add(fk);
            }
        }

        return res;
    }

    // TODO: call delete API
    public IEnumerable<string> GetDeletedKeys()
    {
        return _deletedKeys.Where(key => !_keysDict.Keys.Contains(key));
    }

    internal static Dictionary<string, Dictionary<string, Permissions>> CreateStructuredPermissions(FeatureKey[] allKeys)
    {
        var keysDict = new Dictionary<string, Dictionary<string, Permissions>>();

        foreach (var key in allKeys)
        {
            keysDict.TryGetValue(key.Name, out var rolesDict);

            rolesDict ??= [];

            foreach (var rp in key.RolePermissions)
            {
                rolesDict[rp.RoleName] = rp.Permissions;
            }

            keysDict[key.Name] = rolesDict;
        }

        return keysDict;
    }
}
