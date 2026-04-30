using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ProcurmentProject.Repositories
{
    public class PurchasedRequisitionRepository : IPurchasedRequisition
    {
        private readonly ProcurmentSystemContext _context;
        public PurchasedRequisitionRepository(ProcurmentSystemContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> CreatePrRequest(int userId, PurchasedRequisitionDto prRequest)
        {
            if (prRequest == null)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Valid Data" };
            }
            var request = new PurchasedRequisition
            {
               Quantity = prRequest.Quantity,
               EstimatedBudget = prRequest.Estimated_budget,
               Title = prRequest.Title,
               DeliveryDate = prRequest.DeliveryDate,
               Note = prRequest.Note,
               Deleted = 0,
               CreatedAt = DateTime.UtcNow,
               UpdatedAt = DateTime.UtcNow,
               UserId = userId

            };
            _context.PurchasedRequisitions.Add(request);
            await _context.SaveChangesAsync();

            return new ResponseModel { Success = true, Message = "Successfully Created Pr", Id = request.Id };
        }
        
        public async Task<ResponseModel> UpdatePrRequest(int prId,PurchasedRequisitionDto prRequest)
        {
            if(prRequest == null)
            {
                return new ResponseModel { Success = false, Message = "Please Enter Valid Data" };
            }
            var request =  _context.PurchasedRequisitions.Where(x => x.Deleted==0 && x.Id== prId).FirstOrDefault();
            if (request == null)
            {
                return new ResponseModel { Success = false, Message = "Invalid Id" };
            }
            request.Quantity = prRequest.Quantity;
            request.EstimatedBudget = prRequest.Estimated_budget;
            request.Title = prRequest.Title;
            request.DeliveryDate = prRequest.DeliveryDate;
            request.Note = prRequest.Note;
            request.UpdatedAt = DateTime.UtcNow;
           

            _context.PurchasedRequisitions.Update(request);
            await _context.SaveChangesAsync();

            return new ResponseModel { Success = true, Message = "Successfully Created Pr", Id = request.Id };
        }
        public async Task<ResponseModel> GetPrRequest(int? prId = null)
        {
            var query =  _context.PurchasedRequisitions
                .Where(pr => pr.Deleted == 0);
            if(prId.HasValue)
            {
                query = query.Where(pr => pr.Id == prId.Value);
            }

             var prRequestNew =await query.Select(pp => new
                {
                    PrId = pp.Id,
                    Title = pp.Title,
                    quantity = pp.Quantity,
                    estimationBudget = pp.EstimatedBudget,
                    deliveryDate = pp.DeliveryDate,
                    createdDate = pp.CreatedAt,

                    Product = pp.PrProducts
                   .Where(prProd => prProd.Deleted == 0)
                   .Select(pr => new
                   {
                       productId = pr.ProductId,
                       productName = pr.Product!.Name,
                       company = pr.Product.Company
                   }).ToList()
                })
                .ToListAsync();
            var prRequest = prRequestNew.Distinct().ToList();
            if (prRequest.Count == 0)
            {
                return new ResponseModel { Success = false, Message = "No PR Found" };
            }
            return new ResponseModel
            {
                Success = true,
                Message = "PR Data Fetched",
                Data = prRequest
            };
        }

        public async Task<ResponseModel> DeletePrRequest(int prId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            var request = _context.PurchasedRequisitions.Where(x => x.Id == prId).FirstOrDefault();
            try
            {
                if (request == null)
                {
                    return new ResponseModel { Success = false, Message = "the id is not valid" };
                }
                request.Deleted = 1;
                var products = await _context.PrProducts.Where(pp => pp.PrId == request.Id && pp.Deleted == 0)
                    .Include(pp => pp.Product).
                    Select(pp => pp.Product).ToListAsync();
                foreach (var prod in products)
                {
                    prod.Deleted = 1;
                }
                var prLink = await _context.PrProducts.Where(pp => pp.PrId == request.Id).ToListAsync();
                foreach (var link in prLink)
                {
                    link.Deleted = 1;
                }

                await _context.SaveChangesAsync();
                transaction.CommitAsync();
                return new ResponseModel { Success = true, Message = "successfully deleted product" };

            } catch
            {
                transaction.Rollback();
                return new ResponseModel { Success = false, Message = "Pr is not deleted properly" };
            }
          
        }
    }
}
