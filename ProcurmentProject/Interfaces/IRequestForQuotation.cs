using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface IRequestForQuotation
    {
        public Task<(bool success, string message)> CreateRfq(int PrId, RfqDto rfqDto);
        public Task<(bool success, string message)> SendQuotationToAllSupplier(int rfqId);
        public Task<(bool success, string message)> SendQuotationToSpecificSupplier(List<int> supplierId,int rfqId);
        public Task<(bool success, string message, object? rfqs)> GetAllRfqs();
        public Task<(bool success, string message)> UpdateRfq(int rfqId, RfqDto rfqDto);
        public Task<(bool success, string message)> DeleteRfq(int rfqId);
    }
}
