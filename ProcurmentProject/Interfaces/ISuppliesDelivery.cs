using Microsoft.AspNetCore.Http;
using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface ISuppliesDelivery
    {
        public Task<ResponseModel> AddSuppliesDelivery(int rfqId, int supplierId, SupplierDeliveryDto supplierDeliveryDto);
        public Task<ResponseModel> GetSuppliesDelivery(int? suppliesDeliveryId = null);
        public Task<ResponseModel> GetSupplierDeliveryView();
        public Task<ResponseModel> UpdateSuppliesDelivery(int suppliesDeliveryId, SupplierDeliveryDto supplierDeliveryDto);
        public Task<ResponseModel> DeleteSuppliesDelivery(int suppliesDeliveryId);
    }
}
