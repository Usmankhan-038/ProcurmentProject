using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;
using ProcurmentProject.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> CreatePrRequest(CreatePurchaseRequisitionDto pr)
        {
            string userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            var prResult = await _pr.CreatePrRequest(Int32.Parse(userId),pr.prRequest);
            if(prResult.success)
            {
                foreach (var prod in pr.products)
                {
                    var productResult = await _product.AddProduct(prod);
                    if(productResult.success)
                    {
                        var prProduct = await _product.AddPrProduct(productResult.productId.Value, prResult.prId.Value);
                        if(!prProduct.success)
                        {
                            return BadRequest(prProduct.message);
                        }
                    }
                    else
                    {
                        return BadRequest(productResult.message);
                    }
                }
            } else
            {
                return BadRequest(prResult.message);
            }


            return Ok("Pr Created Successfully");
        }
        [HttpPost("update-pr")]
        public async Task<IActionResult> UpdatePrRequest(int prId,PurchasedRequisitionDto pr)
        {
            if(prId == null)
            {
                return BadRequest("Please Enter the Correct Id");
            }
            var result = await _pr.UpdatePrRequest(prId, pr);

            if(!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpGet("get-all-pr-request")]
        public async Task<IActionResult> GetAllPrRequest()
        {
            var prRequest = await _pr.GetPrRequest();
            if(!prRequest.success)
            {
                return BadRequest(prRequest.message);
            }
            return Ok(new
            {
                message = prRequest.message,
                prRequests = prRequest.prRequest
            });
        }

        [HttpGet("get-pr-request")]
        public async Task<IActionResult> GetAllPrRequest(int prId)
        {
            var prRequest = await _pr.GetPrRequest(prId);
            if (!prRequest.success)
            {
                return BadRequest(prRequest.message);
            }
            return Ok(new
            {
                message = prRequest.message,
                prRequests = prRequest.prRequest
            });
        }

        [HttpDelete("delete-pr-request")]
        public async Task<IActionResult> DeletePrRequest(int prId)
        {
            var prRequest = await _pr.DeletePrRequest(prId);
            if (!prRequest.success)
            {
                return BadRequest(prRequest.message);
            }
            return Ok( prRequest.message );
        }

    }
}
