using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Dto;
using ProcurmentProject.Filters;
using ProcurmentProject.Interfaces;

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuppliesDeliveryController : ControllerBase
    {
        private readonly ISuppliesDelivery _suppliesDelivery;
        public SuppliesDeliveryController(ISuppliesDelivery suppliesDelivery)
        {
            _suppliesDelivery = suppliesDelivery;
        }

        [HttpPost("add-supplies-delivery")]
        [HasPermission("supplier_delivery", "create")]

        public async Task<IActionResult> AddSuppliesDelivery(int rfqId, int supplierId, [FromForm] SupplierDeliveryDto supplierDeliveryDto)
        {
            var result = await _suppliesDelivery.AddSuppliesDelivery(rfqId, supplierId, supplierDeliveryDto);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpGet("get-supplies-delivery")]
        [HasPermission("supplier_delivery", "read")]

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
        [HasPermission("supplier_delivery", "read")]
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
        [HasPermission("supplier_delivery", "update")]
        public async Task<IActionResult> UpdateSuppliesDelivery(int suppliesDeliveryId, [FromForm] SupplierDeliveryDto supplierDeliveryDto)
        {
            var result = await _suppliesDelivery.UpdateSuppliesDelivery(suppliesDeliveryId, supplierDeliveryDto);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpDelete("delete-supplies-delivery")]
        [HasPermission("supplier_delivery", "delete")]
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
