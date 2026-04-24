using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface ISupplier
    {
        public Task<ResponseModel> AddSupplier(SuppliersDto supplier);
        public Task<ResponseModel> UpdateSupplier(int Id, SuppliersDto supplierDto);
        public Task<ResponseModel> DeleteSupplier(int supplierId);
        public Task<ResponseModel> GetAllSupplier();

    }
}
