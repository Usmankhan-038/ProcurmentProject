using Microsoft.EntityFrameworkCore;
using ProcurmentProject.Data.Models;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ProcurmentProject.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly ProcurmentSystemContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(ProcurmentSystemContext context, IMemoryCache cache, ILogger<ProductRepository> logger) 
        {
            _context = context;
            _cache = cache;
            _logger = logger;
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
            var upcExists = await _context.Products.AnyAsync(p => p.Deleted == 0 && p.Upc == prodDto.Upc);
            if (upcExists)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "A product with this UPC already exists."
                };
            }
            try
            {
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
                _cache.Remove("Products");
                return new ResponseModel
                {
                    Success = true,
                    Message = "Product Add successfully",
                    Id = product.Id
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding product with UPC {Upc}.", prodDto.Upc);
                return new ResponseModel
                {
                    Success = false,
                    Message = "Unable to add product. Please verify the data and try again."
                };
            }
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
            var cacheKey = "Products";
            if (!_cache.TryGetValue(cacheKey, out dynamic? product))
            {
                product = await _context.Products.Where(x => x.Deleted == 0).ToListAsync();
                _cache.Set(cacheKey, (object)product, TimeSpan.FromHours(1));
            }
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
            var upcExists = await _context.Products
                .AnyAsync(p => p.Deleted == 0 && p.Upc == product.Upc && p.Id != productId);
            if (upcExists)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "A product with this UPC already exists."
                };
            }
            result.Name = product.Name;
            result.Company = product.Company;
            result.Description = product.Description;
            result.Upc = product.Upc;   
            result.UpdatedAt= DateTime.UtcNow;

            try
            {
                _context.Products.Update(result);
                await _context.SaveChangesAsync();
                _cache.Remove("Products");
                return new ResponseModel { Success = true, Message = "Product Successfully" };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId} with UPC {Upc}.", productId, product.Upc);
                return new ResponseModel
                {
                    Success = false,
                    Message = "Unable to update product. Please verify the data and try again."
                };
            }
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
            _cache.Remove("Products");
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
                Deleted = 0,
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
