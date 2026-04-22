using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliesDeliveryController : ControllerBase
    {
        private readonly ISuppliesDelivery _suppliesDelivery;
        public SuppliesDeliveryController(ISuppliesDelivery suppliesDelivery)
        {
            _suppliesDelivery = suppliesDelivery;
        }

        [HttpPost("add-supplies-delivery")]
        public async Task<IActionResult> AddSuppliesDelivery(int rfqId, int supplierId, [FromForm] SupplierDeliveryDto supplierDeliveryDto, IFormFile? attachment)
        {
            var result = await _suppliesDelivery.AddSuppliesDelivery(rfqId, supplierId, supplierDeliveryDto, attachment);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpGet("get-supplies-delivery")]
        public async Task<IActionResult> GetSuppliesDelivery()
        {
            var result = await _suppliesDelivery.GetSuppliesDelivery();
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(new
            {
                message = result.message,
                suppliesDelivery = result.suppliesDelivery
            });
        }

        [HttpGet("get-supplies-delivery-by-id")]
        public async Task<IActionResult> GetSuppliesDeliveryById(int suppliesDeliveryId)
        {
            var result = await _suppliesDelivery.GetSuppliesDelivery(suppliesDeliveryId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(new
            {
                message = result.message,
                suppliesDelivery = result.suppliesDelivery
            });
        }

        [HttpPost("update-supplies-delivery")]
        public async Task<IActionResult> UpdateSuppliesDelivery(int suppliesDeliveryId, [FromForm] SupplierDeliveryDto supplierDeliveryDto, IFormFile? attachment)
        {
            var result = await _suppliesDelivery.UpdateSuppliesDelivery(suppliesDeliveryId, supplierDeliveryDto, attachment);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpDelete("delete-supplies-delivery")]
        public async Task<IActionResult> DeleteSuppliesDelivery(int suppliesDeliveryId)
        {
            var result = await _suppliesDelivery.DeleteSuppliesDelivery(suppliesDeliveryId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }
    }
}
