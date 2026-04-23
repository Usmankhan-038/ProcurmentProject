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
        public async Task<(bool success, string message, int? productId)> AddProduct(ProductDto prodDto)
        {
            if(prodDto == null)
            {
                return (false, "Please Enter Valid Data", null);
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
            return (true, "Product Add successfully", product.Id);
        }
        public async Task<(bool success, string message, Product? product)> GetProductById(int productId)
        {
            var product = _context.Products.Where(x => x.Deleted == 0).FirstOrDefault();
            if(product == null)
            {
                return (false, "Please Enter Correct Id", null);
            }
            return (true, "Product Fetch Successfully", product);
        }
        public (bool success, string message, Object? product) GetAllProduct()
        {
            var product = _context.Products.Where(x => x.Deleted == 0).ToList();
            if (product != null)
            {
                return (false, "No Product Found", null);
            }
            return (true,"Product Fetch Successfully",  product);
        }
        public async Task<(bool success, string message)> UpdateProduct(int productId,ProductDto product)
        {
            var result = _context.Products.Where(x=>x.Deleted==0 && x.Id == productId).FirstOrDefault();
            if (result == null)
            {
                return (false, "No Product Found with this Id");
            }
            result.Name = product.Name;
            result.Company = product.Company;
            result.Description = product.Description;
            result.Upc = product.Upc;   
            result.UpdatedAt= DateTime.UtcNow;

            _context.Products.Update(result);
            await _context.SaveChangesAsync();
            return (true, "Product Successfully");
        }
        public async Task<(bool success, string message)> DeleteProduct(int productId)
        {
            var result = _context.Products.Find(productId);
            if (result == null)
            {
                return (false, "No Product Found with this id");
            }
            result.Deleted = 1;
            _context.Products.Update(result);
            await _context.SaveChangesAsync();
            return (true, "Product Deleted Successflly");
        }
        public async Task<(bool success, string message)> AddPrProduct(int productId, int prId)
        {
            if(prId == 0 || productId == 0)
            {
                return (true, "Please Provide The correct ids");
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
            return (true, "Product Linked With Pr Product Successfully");
        }
    }
}
