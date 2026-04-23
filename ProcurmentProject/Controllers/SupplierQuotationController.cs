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
    public class SupplierQuotationController : ControllerBase
    {
        private readonly ISupplierQuotation _supplierQuotation;
        public SupplierQuotationController(ISupplierQuotation supplierQuotation)
        {
            _supplierQuotation = supplierQuotation;
        }

        [HttpPost("add-supplier-quotation")]
        [HasPermission("supplier_quotation", "create")]

        public async Task<IActionResult> AddSupplierQuotation(int supplierId, int productId, int rfqId, [FromBody] SupplierQuotationDto supplierQuotationDto)
        {
            var result = await _supplierQuotation.AddSupplierQuotation(supplierId, productId, rfqId, supplierQuotationDto);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpGet("get-supplier-quotation")]
        [HasPermission("supplier_quotation", "read")]

        public async Task<IActionResult> GetSupplierQuotation()
        {
            var result = await _supplierQuotation.GetSupplierQuotation();
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(new 
            {
                message = result.message,
                supplierQuotation = result.supplierQuotation
            });
        }

        [HttpGet("supplier-quotation")]
        [HasPermission("supplier_quotation", "read")]

        public async Task<IActionResult> GetSupplierQuotationById(int quotationId)
        {
            var result = await _supplierQuotation.GetSupplierQuotation(quotationId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(new
            {
                message = result.message,
                supplierQuotation = result.supplierQuotation
            });
        }

        [HttpGet("get-supplier-quotation-by-product-id")]
        [HasPermission("supplier_quotation", "read")]

        public async Task<IActionResult> GetSupplierQuotationByProductId(int productId)
        {
            var result = await _supplierQuotation.GetSupplierQuotationByProductId(productId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(new
            {
                message = result.message,
                supplierQuotation = result.supplierQuotation
            });
        }

        [HttpPost("update-supplier-quotation")]
        [HasPermission("supplier_quotation", "update")]

        public async Task<IActionResult> UpdateSupplierQuotation(int quotationId, [FromBody] SupplierQuotationDto supplierQuotationDto)
        {
            var result = await _supplierQuotation.UpdateSupplierQuotation(quotationId, supplierQuotationDto);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpDelete("delete-supplier-quotation")]
        [HasPermission("supplier_quotation", "delete")]

        public async Task<IActionResult> DeleteSupplierQuotation(int quotationId)
        {
            var result = await _supplierQuotation.DeleteSupplierQuotation(quotationId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }
    }
}
