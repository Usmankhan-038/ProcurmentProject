using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Dto;
using Microsoft.AspNetCore.Authorization;
using ProcurmentProject.Filters;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccessRoleController : ControllerBase
    {
        private readonly IRole _role;
        private readonly IMemoryCache _cache;
        public AccessRoleController(IRole role, IMemoryCache cache)
        {
            _role = role;
            _cache = cache;
        }

        [HttpPost("add-role")]
        [HasPermission("role","create")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var accessRole = await _role.AddAccessRole(role);
            if (!accessRole.Success)
            {
                return BadRequest(accessRole.Message);
            }
           
            return Ok(accessRole);
        }

        [HttpGet("get-userroles")]
        [HasPermission("role", "read")]

        public async Task<IActionResult> GetUserRole()
        {
            var userRole = await _role.GetAllUserRole();
            if (!userRole.Success)
            {
                return BadRequest(userRole.Message);
            }
            return Ok(userRole);
        }
        [HttpDelete("delete-role")]
        [HasPermission("role", "delete")]

        public async Task<IActionResult> DeleteRole([FromBody] int id)
        {
            var deletedRole = await _role.DeleteRole(id);
            if (!deletedRole.Success)
            {
                return BadRequest(deletedRole.Message);
            }
            _cache.Remove($"perm_{id}");
            return Ok(deletedRole);
        }
   
        [HttpPost("Update-role")]
        [HasPermission("role", "update")]

        public async Task<IActionResult> UpdateRole(int id,[FromForm] RoleDto role)
        {
            var updateRole = await _role.UpdateRole(id,role);
            if (!updateRole.Success)
            {
                return BadRequest(updateRole.Message);
            }
            _cache.Remove($"perm_{id}");
            return Ok(updateRole);
        }
    }
}
