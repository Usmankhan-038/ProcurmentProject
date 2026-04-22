using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProcurmentProject.Helper;
using ProcurmentProject.Interfaces;
using System.Security.Claims;


namespace ProcurmentProject.Filters
{
    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private string _module;
        private string _action;
        private readonly IRole _role;
        private readonly PermissionChecker _permissionChecker;

        public PermissionFilter(string module, string action,IRole role, PermissionChecker permissionChecker)
        {
            _module = module;
            _action = action;
            _role = role;
            _permissionChecker = permissionChecker;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var claimUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimUserId) || !int.TryParse(claimUserId, out int userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            var rolePermission = await _role.GetPermissionByUserId(userId);

            if(!rolePermission.success  || rolePermission.permission == null || !_permissionChecker.HasPermission(rolePermission.permission,_module,_action))
            {
                context.Result = new JsonResult(new { message = "Unauthorized Access" });
            }
        }
    }
}
