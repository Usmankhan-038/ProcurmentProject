using ProcurmentProject.Models;

namespace ProcurmentProject.Helper
{
    public class CachKeys
    {
        public static string UserPermissionKey(int id) => $"perm_role_{id}";
    }
}
