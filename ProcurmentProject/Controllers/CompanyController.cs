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
        [HttpPost("add-company")]
        public async Task<ActionResult> AddCompany(CompanyDto company)
        { 
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companyAdded = await _company.AddCompany(company);
            if(!companyAdded.Success)
            {
                return BadRequest(companyAdded.Message);
            }
            return Ok(companyAdded);
        }
        [Authorize]
        [HttpGet("companies")]
        public async Task<ActionResult> GetAllCompany()
        { 
            var companyList = await _company.GetAllCompany();
            if (!companyList.Success)
            {
                return BadRequest(companyList.Message);
            }
            return Ok(companyList);
        }

    }
}
