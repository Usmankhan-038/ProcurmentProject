using Microsoft.AspNetCore.Http;
using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface ISuppliesDelivery
    {
        public Task<(bool success, string message)> AddSuppliesDelivery(int rfqId, int supplierId, SupplierDeliveryDto supplierDeliveryDto);
        public Task<(bool success, string message, Object? suppliesDelivery)> GetSuppliesDelivery(int? suppliesDeliveryId = null);
        public Task<(bool success, string message)> UpdateSuppliesDelivery(int suppliesDeliveryId, SupplierDeliveryDto supplierDeliveryDto);
        public Task<(bool success, string message)> DeleteSuppliesDelivery(int suppliesDeliveryId);
    }
}
