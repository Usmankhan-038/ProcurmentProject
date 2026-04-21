using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;
using ProcurmentProject.Data;
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

        public async Task<(bool success, string message, int? prId)> CreatePrRequest(int userId, PurchasedRequisitionDto prRequest)
        {
            if (prRequest == null)
            {
                return (false, "Please Provide Valid Data", null);
            }
            var request = new PurchasedRequisition
            {
               Quantity = prRequest.quantity,
               EstimatedBudget = prRequest.estimated_budget,
               Title = prRequest.title,
               DeliveryDate = prRequest.deliveryDate,
               Note = prRequest.note,
               CreatedAt = DateTime.UtcNow,
               UpdatedAt = DateTime.UtcNow,
               UserId = userId

            };
            _context.PurchasedRequisitions.Add(request);
            await _context.SaveChangesAsync();

            return (true, "Successfully Created Pr", request.Id);
        }
        
        public async Task<(bool success, string message, int? prId)> UpdatePrRequest(int prId,PurchasedRequisitionDto prRequest)
        {
            if(prRequest == null)
            {
                return (false, "Please Enter Valid Data",null);
            }
            var request =  _context.PurchasedRequisitions.Where(x => x.Deleted==0 && x.Id== prId).FirstOrDefault();
            if (request == null)
            {
                return (false, "Invalid Id", null);
            }
            request.Quantity = prRequest.quantity;
            request.EstimatedBudget = prRequest.estimated_budget;
            request.Title = prRequest.title;
            request.DeliveryDate = prRequest.deliveryDate;
            request.Note = prRequest.note;
            request.UpdatedAt = DateTime.UtcNow;
           

            _context.PurchasedRequisitions.Update(request);
            await _context.SaveChangesAsync();

            return (true, "Successfully Created Pr", request.Id);
        }
        public async Task<(bool success, string message, Object? prRequest)> GetPrRequest(int? prId = null)
        {
            var query =  _context.PurchasedRequisitions
                .Where(pr => pr.Deleted == 0);
            if(prId.HasValue)
            {
                query = query.Where(pr => pr.Id == prId.Value);
            }

             var prRequest =await query.Select(pp => new
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
                       productName = pr.Product.Name,
                       company = pr.Product.Company
                   }).ToList()
                }).ToListAsync();

            if (prRequest == null)
            {
                return (false, "No PR Found", null);
            }
            return (true, "PR Data Fetched", prRequest);
        }

        public async Task<(bool success, string message)> DeletePrRequest(int prId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            var request = _context.PurchasedRequisitions.Where(x => x.Id == prId).FirstOrDefault();
            try
            {
                if (request == null)
                {
                    return (false, "the id is not valid");
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
                return (true, "successfully deleted product");

            } catch
            {
                transaction.Rollback();
                return (false, "Pr is not deleted properly");
            }
          
        }
    }
}
