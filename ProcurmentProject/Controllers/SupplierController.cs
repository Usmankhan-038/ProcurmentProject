using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Dto;
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
        public async Task<IActionResult> AddSupplier([FromBody] SuppliersDto suppliersDto)
        {
            var supplier = await _supplier.AddSupplier(suppliersDto);
            if(!supplier.success)
            {
                return BadRequest(supplier.message);
            }
            return Ok(supplier.message);
        }
        [HttpPost("update-supplier")]
        public async Task<IActionResult> UpdateSupplier( int Id,[FromBody] SuppliersDto suppliersDto)
        {
            var supplier = await _supplier.UpdateSupplier(Id,suppliersDto);
            if (!supplier.success)
            {
                return BadRequest(supplier.message);
            }
            return Ok(supplier.message);
        }

        [HttpDelete("delete-supplier")]
        public async Task<IActionResult> DeleteSupplier([FromBody] int Id)
        {
            var supplier = await _supplier.DeleteSupplier(Id);
            if (!supplier.success)
            {
                return BadRequest(supplier.message);
            }
            return Ok(supplier.message);
        }

        [HttpGet("get-supplier")]
        public async Task<IActionResult> GetSupplier()
        {
            var supplier = await _supplier.GetAllSupplier();
            if (!supplier.success)
            {
                return BadRequest(supplier.message);
            }
            return Ok(new { 
                message = supplier.message,
                supplier = supplier.supplier
            });
        }
    }

}
