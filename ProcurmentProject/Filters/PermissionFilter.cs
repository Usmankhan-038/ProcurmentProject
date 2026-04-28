using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProcurmentProject.Helper;
using ProcurmentProject.Interfaces;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;


namespace ProcurmentProject.Filters
{
    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private string _module;
        private string _action;
        private readonly IRole _role;
        private readonly PermissionChecker _permissionChecker;
        private readonly IMemoryCache _cache;
        public PermissionFilter(string module, string action,IRole role, PermissionChecker permissionChecker, IMemoryCache cache)
        {
            _module = module;
            _action = action;
            _role = role;
            _permissionChecker = permissionChecker;
            _cache = cache;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var claimUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
            if (claimUserId == null)
            {
                context.Result = new ForbidResult();
                return;
            }
            var userId = int.Parse(claimUserId!);
            var claimRoleId = user.FindFirst("role_id")?.Value;
            if (claimRoleId == null || !int.TryParse(claimRoleId, out var roleId))
            {
                context.Result = new JsonResult(new { message = "Unauthorized Access" }) { StatusCode = 403 };
                return;
            }
            string cacheKey = $"perm_role_{roleId}";
            if (!_cache.TryGetValue(cacheKey, out string? permissionJson))
            {

                var rolePermission = await _role.GetPermissionByUserId(userId);
                if (!rolePermission.Success || rolePermission.Data == null)
                {
                    context.Result = new JsonResult(new { message = "Unauthorized Access" }) { StatusCode = 403 };
                    return;
                }

                permissionJson = rolePermission.Data.ToString();

                _cache.Set(cacheKey, permissionJson, TimeSpan.FromHours(1));
            }


            if(!_permissionChecker.HasPermission(permissionJson!, _module, _action))
            {
                context.Result = new JsonResult(new { message = "Unauthorized Access" });
            }
        }
    }
}
