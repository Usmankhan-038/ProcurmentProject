using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface ISupplierQuotation
    {
        public Task<(bool success, string message)> AddSupplierQuotation(int supplierId, int productId, int rfqId, SupplierQuotationDto supplierQuotationDto);
        public Task<(bool success, string message, Object? supplierQuotation)> GetSupplierQuotation(int? quotationId = null);
        public Task<(bool success, string message, Object? supplierQuotation)> GetSupplierQuotationByProductId(int productId);
        public Task<(bool success, string message)> UpdateSupplierQuotation(int quotationId, SupplierQuotationDto supplierQuotationDto);
        public Task<(bool success, string message)> DeleteSupplierQuotation(int quotationId);
    }
}
