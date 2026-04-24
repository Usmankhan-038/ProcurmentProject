using ProcurmentProject.Dto;

namespace ProcurmentProject.Interfaces
{
    public interface IRequestForQuotation
    {
        public Task<ResponseModel> CreateRfq(int PrId, RfqDto rfqDto);
        public Task<ResponseModel> SendQuotationToAllSupplier(int rfqId);
        public Task<ResponseModel> SendQuotationToSpecificSupplier(List<int> supplierId,int rfqId);
        public Task<ResponseModel> GetAllRfqs();
        public Task<ResponseModel> UpdateRfq(int rfqId, RfqDto rfqDto);
        public Task<ResponseModel> DeleteRfq(int rfqId);
    }
}
