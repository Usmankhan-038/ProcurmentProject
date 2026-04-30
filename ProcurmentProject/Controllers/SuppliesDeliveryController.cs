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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _suppliesDelivery.AddSuppliesDelivery(rfqId, supplierId, supplierDeliveryDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("get-supplier-delivery-view")]
        [HasPermission("supplies_delivery", "read")]
        public async Task<IActionResult> GetSupplierDeliveryView()
        {
            var result = await _suppliesDelivery.GetSupplierDeliveryView();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("get-supplies-delivery")]
        [HasPermission("supplier_delivery", "read")]

        public async Task<IActionResult> GetSuppliesDelivery()
        {
            var result = await _suppliesDelivery.GetSuppliesDelivery();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("get-supplies-delivery-by-id")]
        [HasPermission("supplier_delivery", "read")]
        public async Task<IActionResult> GetSuppliesDeliveryById(int suppliesDeliveryId)
        {
            var result = await _suppliesDelivery.GetSuppliesDelivery(suppliesDeliveryId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("update-supplies-delivery")]
        [HasPermission("supplier_delivery", "update")]
        public async Task<IActionResult> UpdateSuppliesDelivery(int suppliesDeliveryId, [FromForm] SupplierDeliveryDto supplierDeliveryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _suppliesDelivery.UpdateSuppliesDelivery(suppliesDeliveryId, supplierDeliveryDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("delete-supplies-delivery")]
        [HasPermission("supplier_delivery", "delete")]
        public async Task<IActionResult> DeleteSuppliesDelivery(int suppliesDeliveryId)
        {
            var result = await _suppliesDelivery.DeleteSuppliesDelivery(suppliesDeliveryId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
