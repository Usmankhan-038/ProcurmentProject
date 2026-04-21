using System.Text.Json;

namespace ProcurmentProject.Helper
{
    public class PermissionChecker
    {
        public bool  HasPermission(string jsonPermission, string module, string action)
        {
            try
            {
                var permission = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonPermission);

                if (permission != null && permission.ContainsKey(module))
                {
                    return permission[module].ContainsKey(action) && permission[module][action].ToLower() == "allowed";
                }
            } catch
            {
                return false;
            }
           
            return false;
        }
    }
}
