using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Dto;
using Microsoft.AspNetCore.Authorization;
using ProcurmentProject.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccessRoleController : ControllerBase
    {
        private readonly IRole _role;
        public AccessRoleController(IRole role)
        {
            _role = role;
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
            if (!accessRole.success)
            {
                return BadRequest(accessRole.message);
            }
            return Ok(accessRole.message);
        }

        [HttpGet("get-userroles")]
        [HasPermission("role", "read")]

        public async Task<IActionResult> GetUserRole()
        {
            var userRole = await _role.GetAllUserRole();
            return Ok(userRole);
        }
        [HttpDelete("delete-role")]
        [HasPermission("role", "delete")]

        public async Task<IActionResult> DeleteRole([FromBody] int id)
        {
            var deletedRole = await _role.DeleteRole(id);
            if (!deletedRole.success)
            {
                return BadRequest(deletedRole.message);
            }
            return Ok(deletedRole.message);
        }
   
        [HttpPost("Update-role")]
        [HasPermission("role", "update")]

        public async Task<IActionResult> UpdateRole(int id,[FromForm] RoleDto role)
        {
            var updateRole = await _role.UpdateRole(id,role);
            if (!updateRole.success)
            {
                return BadRequest(updateRole.message);
            }
            return Ok(updateRole.message);
        }
    }
}
