using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface ISupplierQuotation
    {
        public Task<ResponseModel> AddSupplierQuotation(int supplierId, int productId, int rfqId, SupplierQuotationDto supplierQuotationDto);
        public Task<ResponseModel> GetSupplierQuotation(int? quotationId = null);
        public Task<ResponseModel> GetSupplierQuotationByProductId(int productId);
        public Task<ResponseModel> UpdateSupplierQuotation(int quotationId, SupplierQuotationDto supplierQuotationDto);
        public Task<ResponseModel> DeleteSupplierQuotation(int quotationId);
    }
}
