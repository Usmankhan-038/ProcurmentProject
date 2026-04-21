using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface ISupplier
    {
        public Task<(bool success, string message)> AddSupplier(SuppliersDto supplier);
        public Task<(bool success, string message)> UpdateSupplier(int Id, SuppliersDto supplierDto);
        public Task<(bool success, string message)> DeleteSupplier(int supplierId);
        public Task<(bool success, string message, Object? supplier)> GetAllSupplier();

    }
}
