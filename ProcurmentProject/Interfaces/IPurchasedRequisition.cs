using ProcurmentProject.Dto;
using ProcurmentProject.Models;

namespace ProcurmentProject.Interfaces
{
    public interface IPurchasedRequisition
    {
        public Task<(bool success, string message, int? prId)> CreatePrRequest(int userId, PurchasedRequisitionDto prRequest);
        public Task<(bool success, string message, Object? prRequest)> GetPrRequest(int? prId = null);
        public Task<(bool success, string message, int? prId)> UpdatePrRequest(int prId,PurchasedRequisitionDto prRequest);
        public Task<(bool success, string message)> DeletePrRequest(int prId);

    }
}
