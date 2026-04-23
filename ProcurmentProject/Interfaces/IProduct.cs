using ProcurmentProject.Models;
using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface IProduct
    {
        public Task<(bool success, string message,int? productId)> AddProduct(ProductDto prodDto);
        public Task<(bool success,string message, Product? product)> GetProductById(int productId);
        public (bool success, string message, Object? product) GetAllProduct();
        public Task<(bool success,string message)> UpdateProduct(int productId, ProductDto product);
        public Task<(bool success, string message)> DeleteProduct(int productId);
        public Task<(bool success, string message)> AddPrProduct(int productId, int prId);
    }
}
