using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using System.Threading.Tasks;
using System.Security.Claims;
using ProcurmentProject.Helper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompany _company;
        private readonly IRole _role;
        private readonly PermissionChecker _permissionChecker;
        public CompanyController(ICompany company, IRole role,PermissionChecker permissionChecker) 
        {
            _company = company;
            _role = role;
            _permissionChecker = permissionChecker;
        }
        [Authorize]
        [HttpPost("add-Company")]
        public async Task<ActionResult> AddCompany(CompanyDto company)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            string permission;
            var flag = int.TryParse(userId, out int result);
           
            var rolePermission = await _role.GetPermissionByUserId(result);
            permission = rolePermission.permission;

            
            var userAuthorization = _permissionChecker.HasPermission(permission, "company", "create");

            if(!userAuthorization)
            {
                return StatusCode(403,"Unauthorized Request");
            }
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companyAdded = await _company.addCompany(company);
            if(!companyAdded.success)
            {
                return BadRequest(companyAdded.message);
            }
            return Ok(companyAdded.message);
        }
        [Authorize]
        [HttpGet("get-companies")]
        public async Task<ActionResult> GetAllCompany()
        { 
            var companyList = await _company.getAllCompany();
            return Ok(companyList);
        }

    }
}
