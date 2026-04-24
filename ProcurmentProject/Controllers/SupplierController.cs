using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Dto;
using ProcurmentProject.Filters;
using ProcurmentProject.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplier _supplier;
        public SupplierController(ISupplier supplier)
        {
            _supplier = supplier;
        }

        [HttpPost("add-supplier")]
        [HasPermission("supplier", "create")]

        public async Task<IActionResult> AddSupplier([FromBody] SuppliersDto suppliersDto)
        {
            var supplier = await _supplier.AddSupplier(suppliersDto);
            if(!supplier.Success)
            {
                return BadRequest(supplier.Message);
            }
            return Ok(supplier);
        }
        [HttpPost("update-supplier")]
        [HasPermission("supplier", "Update")]

        public async Task<IActionResult> UpdateSupplier( int Id,[FromBody] SuppliersDto suppliersDto)
        {
            var supplier = await _supplier.UpdateSupplier(Id,suppliersDto);
            if (!supplier.Success)
            {
                return BadRequest(supplier.Message);
            }
            return Ok(supplier);
        }

        [HttpDelete("delete-supplier")]
        [HasPermission("supplier", "delete")]

        public async Task<IActionResult> DeleteSupplier([FromBody] int Id)
        {
            var supplier = await _supplier.DeleteSupplier(Id);
            if (!supplier.Success)
            {
                return BadRequest(supplier.Message);
            }
            return Ok(supplier);
        }

        [HttpGet("get-supplier")]
        [HasPermission("supplier", "read")]

        public async Task<IActionResult> GetSupplier()
        {
            var supplier = await _supplier.GetAllSupplier();
            if (!supplier.Success)
            {
                return BadRequest(supplier.Message);
            }
            return Ok(supplier);
        }
    }

}
