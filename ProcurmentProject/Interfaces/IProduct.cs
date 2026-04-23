using ProcurmentProject.Models;
using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface IProduct
    {
        public Task<ResponseModel> AddProduct(ProductDto prodDto);
        public Task<ResponseModel> GetProductById(int productId);
        public ResponseModel GetAllProduct();
        public Task<ResponseModel> UpdateProduct(int productId, ProductDto product);
        public Task<ResponseModel> DeleteProduct(int productId);
        public Task<ResponseModel> AddPrProduct(int productId, int prId);
    }
}
