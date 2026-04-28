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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _rfq.CreateRfq(prId, rfqDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("send-quotation-to-all-supplier")]
        [HasPermission("rfq", "create")]

        public async Task<IActionResult> SendQuotationToAllSupplier(int rfqId)
        {
            var result = await _rfq.SendQuotationToAllSupplier(rfqId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("send-quotation-to-specific-supplier")]
        [HasPermission("rfq", "create")]

        public async Task<IActionResult> SendQuotationToSpecificSupplier([FromBody] List<int> supplierId, int rfqId)
        {
            var result = await _rfq.SendQuotationToSpecificSupplier(supplierId, rfqId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("get-rfqs")]
        [HasPermission("rfq", "read")]

        public async Task<IActionResult> GetAllRfqs()
        {
            try
            {
                var result = await _rfq.GetAllRfqs();
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("update-rfq")]
        [HasPermission("rfq", "update")]

        public async Task<IActionResult> UpdateRfq(int rfqId, [FromForm] RfqDto rfqDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _rfq.UpdateRfq(rfqId, rfqDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("delete-rfq")]
        [HasPermission("rfq", "delete")]

        public async Task<IActionResult> DeleteRfq(int rfqId)
        {
            var result = await _rfq.DeleteRfq(rfqId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
