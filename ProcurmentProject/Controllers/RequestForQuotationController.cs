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
    public class RequestForQuotationController : ControllerBase
    {
        private readonly IRequestForQuotation _rfq;

        public RequestForQuotationController(IRequestForQuotation rfq)
        {
            _rfq = rfq;
        }

        [HttpPost("create-rfq")]
        [HasPermission("rfq", "create")]

        public async Task<IActionResult> CreateRfq(int prId, [FromForm] RfqDto rfqDto)
        {
            var result = await _rfq.CreateRfq(prId, rfqDto);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpPost("send-quotation-to-all-supplier")]
        [HasPermission("rfq", "create")]

        public async Task<IActionResult> SendQuotationToAllSupplier(int rfqId)
        {
            var result = await _rfq.SendQuotationToAllSupplier(rfqId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpPost("send-quotation-to-specific-supplier")]
        [HasPermission("rfq", "create")]

        public async Task<IActionResult> SendQuotationToSpecificSupplier([FromBody] List<int> supplierId, int rfqId)
        {
            var result = await _rfq.SendQuotationToSpecificSupplier(supplierId, rfqId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpGet("get-rfqs")]
        [HasPermission("rfq", "read")]

        public async Task<IActionResult> GetAllRfqs()
        {
            var result = await _rfq.GetAllRfqs();
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(new
            {
                message = result.message,
                rfqs = result.rfqs
            });
        }

        [HttpPost("update-rfq")]
        [HasPermission("rfq", "update")]

        public async Task<IActionResult> UpdateRfq(int rfqId, [FromForm] RfqDto rfqDto)
        {
            var result = await _rfq.UpdateRfq(rfqId, rfqDto);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpDelete("delete-rfq")]
        [HasPermission("rfq", "delete")]

        public async Task<IActionResult> DeleteRfq(int rfqId)
        {
            var result = await _rfq.DeleteRfq(rfqId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }
    }
}
