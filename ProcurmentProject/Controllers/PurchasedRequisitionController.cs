using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Dto;
using ProcurmentProject.Filters;
using ProcurmentProject.Interfaces;
using System.Security.Claims;

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchasedRequisitionController : ControllerBase
    {
        private readonly IPurchasedRequisition _pr;
        private readonly IProduct _product;

        public PurchasedRequisitionController(IPurchasedRequisition pr, IProduct product)
        {
            _pr = pr;
            _product = product;
        }

        [HttpPost]
        [Authorize]
        [HasPermission("pr_module", "create")]

        public async Task<IActionResult> CreatePrRequest(CreatePurchaseRequisitionDto pr)
        {
            string userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            var prResult = await _pr.CreatePrRequest(Int32.Parse(userId),pr.PrRequest);
            if(prResult.Success)
            {
                foreach (var prod in pr.Products)
                {
                    var productResult = await _product.AddProduct(prod);
                    if(productResult.Success)
                    {
                        var prProduct = await _product.AddPrProduct(productResult.Id!.Value, prResult.Id!.Value);
                        if(!prProduct.Success)
                        {
                            return BadRequest(prProduct.Message);
                        }
                    }
                    else
                    {
                        return BadRequest(productResult.Message);
                    }
                }
            } else
            {
                return BadRequest(prResult.Message);
            }

            return Ok(new ResponseModel { Success = true, Message = "Pr Created Successfully", Id = prResult.Id });
        }
        [HttpPost("update-pr")]
        [HasPermission("pr_module", "update")]

        public async Task<IActionResult> UpdatePrRequest(int prId,PurchasedRequisitionDto pr)
        {
            if(prId == null)
            {
                return BadRequest("Please Enter the Correct Id");
            }
            var result = await _pr.UpdatePrRequest(prId, pr);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("get-all-pr-request")]
        [HasPermission("pr_module", "read")]

        public async Task<IActionResult> GetAllPrRequest()
        {
            var prRequest = await _pr.GetPrRequest();
            if(!prRequest.Success)
            {
                return BadRequest(prRequest.Message);
            }
            return Ok(prRequest);
        }

        [HttpGet("get-pr-request-byId")]
        [HasPermission("pr_module", "read")]

        public async Task<IActionResult> GetAllPrRequestById(int prId)
        {
            var prRequest = await _pr.GetPrRequest(prId);
            if (!prRequest.Success)
            {
                return BadRequest(prRequest.Message);
            }
            return Ok(prRequest);
        }

        [HttpGet("get-pr-detail")]
        [HasPermission("pr_module", "read")]
        public async Task<IActionResult> GetAllPrDetail()
        {
            var prRequest = await _pr.PrCount();
            if (!prRequest.Success)
            {
                return BadRequest(prRequest.Message);
            }
            return Ok(prRequest);
        }
        [HttpDelete("delete-pr-request")]
        [HasPermission("pr_module", "delete")]

        public async Task<IActionResult> DeletePrRequest(int prId)
        {
            var prRequest = await _pr.DeletePrRequest(prId);
            if (!prRequest.Success)
            {
                return BadRequest(prRequest.Message);
            }
            return Ok(prRequest);
        }


    }
}
