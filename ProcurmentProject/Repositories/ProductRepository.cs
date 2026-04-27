using Microsoft.EntityFrameworkCore;
using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;
using System.Threading.Tasks;

namespace ProcurmentProject.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly ProcurmentSystemContext _context;
        public ProductRepository(ProcurmentSystemContext context) 
        {
            _context = context;
        }
        public async Task<ResponseModel> AddProduct(ProductDto prodDto)
        {
            if(prodDto == null)
            {
                return new ResponseModel 
                { 
                    Success = false, 
                    Message = "Please Enter Valid Data" 
                };
            }
            var product = new Product
            {
                Name = prodDto.Name,
                Company = prodDto.Company,
                Description = prodDto.Description,
                Upc = prodDto.Upc,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ResponseModel 
            { 
                Success = true, 
                Message = "Product Add successfully", 
                Id = product.Id 
            };
        }
        public async Task<ResponseModel> GetProductById(int productId)
        {
            var product = await _context.Products.Where(x => x.Deleted == 0).FirstOrDefaultAsync();
            if(product == null)
            {
                return new ResponseModel 
                { 
                    Success = false, 
                    Message = "Please Enter Correct Id" 
                };
            }
            return new ResponseModel 
            { 
                Success = true, 
                Message = "Product Fetch Successfully", 
                Data = product 
            };
        }
        public async Task<ResponseModel> GetAllProduct()
        {
            var product = await _context.Products.Where(x => x.Deleted == 0).ToListAsync();
            if (product == null)
            { 
                return new ResponseModel
                {
                    Success = false,
                    Message = "No Product Found"
                };
            }
            return new ResponseModel
            {
                Success = true,
                Message = "Product Fetch Successfully",
                Data = product
            };
        }
        public async Task<ResponseModel> UpdateProduct(int productId,ProductDto product)
        {
            var result = _context.Products.Where(x=>x.Deleted==0 && x.Id == productId).FirstOrDefault();
            if (result == null)
            {
                return new ResponseModel { 
                    Success = false, 
                    Message = "No Product Found with this Id" 
                };
            }
            result.Name = product.Name;
            result.Company = product.Company;
            result.Description = product.Description;
            result.Upc = product.Upc;   
            result.UpdatedAt= DateTime.UtcNow;

            _context.Products.Update(result);
            await _context.SaveChangesAsync();
            return new ResponseModel { Success = true, Message = "Product Successfully" };
        }
        public async Task<ResponseModel> DeleteProduct(int productId)
        {
            var result = _context.Products.Find(productId);
            if (result == null)
            { 
                return new ResponseModel
                {
                    Success = false,
                    Message = "No Product Found with this id"
                };
            }
            result.Deleted = 1;
            _context.Products.Update(result);
            await _context.SaveChangesAsync();
            return new ResponseModel
            {
                Success = true,
                Message = "Product Deleted Successflly"
            };
           
        }
        public async Task<ResponseModel> AddPrProduct(int productId, int prId)
        {
            if(prId == 0 || productId == 0)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Please Provide The correct ids"
                };
               
            }
            var prProduct = new PrProduct
            {
                PrId = prId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.PrProducts.Add(prProduct);
            await _context.SaveChangesAsync();
            return new ResponseModel
            {
                Success = true,
                Message = "Product Linked With Pr Product Successfully"
            };
        }
    }
}
