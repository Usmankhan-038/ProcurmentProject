using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;

namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _prod;
        public ProductController(IProduct prod)
        {
            _prod = prod;
        }

        [HttpPost("add-Product")]
        public async Task<IActionResult> AddPproduct(ProductDto product)
        {
            if (product == null)
            {
                return BadRequest("Please Enter Product Data"); 
            }
            var result = await _prod.AddProduct(product);
            if (!result.success)
            {
                return BadRequest(result.message);
            }
            return Ok(result.message);
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct()
        {
            var result =  _prod.GetAllProduct();
            if (!result.success)
            {
                return BadRequest(result.message.ToString());
            }

            return Ok(result.message);
        }

        [HttpGet("get-product-by-Id")]
        public async Task<IActionResult> GetProductById(int Id)
        {
            var result = await _prod.getProductById(Id);
            if (!result.success)
            {
                return BadRequest(result.message);
            }

            return Ok(result.message);
        }

        [HttpPost("update-product")]
        public async Task<IActionResult> UpdateProduct(int prodId,ProductDto prod)
        {
            var result = await _prod.UpdateProduct(prodId,prod);
            if (!result.success)
            {
                return BadRequest(result.message);
            }

            return Ok(result.message);
        }

        [HttpDelete("delete-product")]
        public async Task<IActionResult> DeleteProduct(int prodId)
        {
            var result = await _prod.DeleteProduct(prodId);
            if (!result.success)
            {
                return BadRequest(result.message);
            }

            return Ok(result.message);
        }

    }
}
