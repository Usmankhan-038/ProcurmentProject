using ProcurmentProject.Dto;
using ProcurmentProject.Data.Models;

namespace ProcurmentProject.Interfaces
{
    public interface IPurchasedRequisition
    {
        public Task<ResponseModel> CreatePrRequest(int userId, PurchasedRequisitionDto prRequest);
        public Task<ResponseModel> GetPrRequest(int? prId = null);
        public Task<ResponseModel> UpdatePrRequest(int prId,PurchasedRequisitionDto prRequest);
        public Task<ResponseModel> DeletePrRequest(int prId);

    }
}
