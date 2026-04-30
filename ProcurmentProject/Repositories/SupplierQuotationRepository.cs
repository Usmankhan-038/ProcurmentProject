using Microsoft.EntityFrameworkCore;
//using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;

namespace ProcurmentProject.Repositories
{
    public class SupplierQuotationRepository : ISupplierQuotation
    {
        private readonly ProcurmentSystemContext _context;
        public SupplierQuotationRepository(ProcurmentSystemContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> AddSupplierQuotation(int supplierId, int productId, int rfqId, SupplierQuotationDto supplierQuotationDto)
        {
            if (supplierQuotationDto == null)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Valid Data" };
            }

            if (supplierId == 0 || productId == 0 || rfqId == 0)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Correct Id" };
            }

            var supplier = _context.Suppliers.Where(s => s.Deleted == 0 && s.Id == supplierId).FirstOrDefault();
            var product = _context.Products.Where(p => p.Deleted == 0 && p.Id == productId).FirstOrDefault();
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();

            if (supplier == null || product == null || rfq == null)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Correct Id" };
            }

            var quotation = new SupplierQuotation
            {
                SupplierId = supplierId,
                ProductId = productId,
                RfqId = rfqId,
                UnitPrice = supplierQuotationDto.UnitPrice.ToString(),
                Quantity = supplierQuotationDto.SupplierQuantity,
                FinalPrice = supplierQuotationDto.FinalPrice.ToString(),
            };

            _context.SupplierQuotations.Add(quotation);
            await _context.SaveChangesAsync();
            return new ResponseModel { Success = true, Message = "Supplier Quotation Added Successfully", Id = quotation.Id };
        }

        public async Task<ResponseModel> GetSupplierQuotation(int? quotationId = null)
        {
            var query = _context.SupplierQuotations.Where(sq => sq.Deleted == 0);

            if (quotationId.HasValue)
            {
                query = query.Where(sq => sq.Id == quotationId.Value);
            }

            var supplierQuotation = await query.Select(sq => new
            {
                quotationId = sq.Id,
                supplierName = sq.Supplier!.User.Name,
                companyName = sq.Supplier.CompanyName,
                productId = sq.ProductId,
                productName = sq.Product!.Name,
                upc = sq.Product.Upc,
                rfqId = sq.RfqId,
                prTitle = sq.Rfq!.Pr!.Title,
                unitPrice = sq.UnitPrice,
                quantity = sq.Quantity,
                finalPrice = sq.FinalPrice,
                createdAt = sq.CreatedAt
            }).ToListAsync();

            if (supplierQuotation.Count == 0)
            {
                return new ResponseModel { Success = false, Message = "No Supplier Quotation Found" };
            }

            var supplierQuotationWithBids = supplierQuotation
                .GroupBy(sq => new { sq.rfqId, sq.productId })
                .SelectMany(g =>
                {
                    var prices = g.Select(x => ParsePrice(x.finalPrice)).ToList();
                    var lowestBid = prices.Min();
                    var highestBid = prices.Max();

                    return g.Select(x => new
                    {
                        quotationId = x.quotationId,
                        supplierName = x.supplierName,
                        companyName = x.companyName,
                        productName = x.productName,
                        upc = x.upc,
                        prTitle = x.prTitle,
                        unitPrice = x.unitPrice,
                        quantity = x.quantity,
                        finalPrice = x.finalPrice,
                        lowestBid = lowestBid,
                        highestBid = highestBid,
                        isLowestBid = ParsePrice(x.finalPrice) == lowestBid,
                        isHighestBid = ParsePrice(x.finalPrice) == highestBid,
                        createdAt = x.createdAt
                    });
                }).ToList();

            return new ResponseModel
            {
                Success = true,
                Message = "Supplier Quotation Fetch Successfully",
                Data = supplierQuotationWithBids
            };
        }

        public async Task<ResponseModel> GetSupplierQuotationByProductId(int productId)
        {
            var supplierQuotation = await _context.SupplierQuotations
                .Where(sq => sq.Deleted == 0 && sq.ProductId == productId)
                .Select(sq => new
                {
                    quotationId = sq.Id,
                    supplierName = sq.Supplier!.User.Name,
                    companyName = sq.Supplier.CompanyName,
                    productId = sq.ProductId,
                    productName = sq.Product!.Name,
                    upc = sq.Product.Upc,
                    rfqId = sq.RfqId,
                    prTitle = sq.Rfq!.Pr!.Title,
                    unitPrice = sq.UnitPrice,
                    quantity = sq.Quantity,
                    finalPrice = sq.FinalPrice
                }).ToListAsync();

            if (supplierQuotation.Count == 0)
            {
                return new ResponseModel { Success = false, Message = "No Supplier Quotation Found" };
            }

            var supplierQuotationWithBids = supplierQuotation
                .GroupBy(sq => new { sq.rfqId, sq.productId })
                .SelectMany(g =>
                {
                    var prices = g.Select(x => ParsePrice(x.finalPrice)).ToList();
                    var lowestBid = prices.Min();
                    var highestBid = prices.Max();

                    return g.Select(x => new
                    {
                        quotationId = x.quotationId,
                        supplierName = x.supplierName,
                        companyName = x.companyName,
                        productName = x.productName,
                        upc = x.upc,
                        prTitle = x.prTitle,
                        unitPrice = x.unitPrice,
                        quantity = x.quantity,
                        finalPrice = x.finalPrice,
                        lowestBid = lowestBid,
                        highestBid = highestBid,
                        isLowestBid = ParsePrice(x.finalPrice) == lowestBid,
                        isHighestBid = ParsePrice(x.finalPrice) == highestBid
                    });
                }).ToList();

            return new ResponseModel
            {
                Success = true,
                Message = "Supplier Quotation Fetch Successfully",
                Data = supplierQuotationWithBids
            };
        }

        public async Task<ResponseModel> UpdateSupplierQuotation(int quotationId, SupplierQuotationDto supplierQuotationDto)
        {
            if (supplierQuotationDto == null)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Valid Data" };
            }

            var quotation = _context.SupplierQuotations.Where(sq => sq.Deleted == 0 && sq.Id == quotationId).FirstOrDefault();
            if (quotation == null)
            {
                return new ResponseModel { Success = false, Message = "No Supplier Quotation Found" };
            }

            quotation.UnitPrice = supplierQuotationDto.UnitPrice.ToString();
            quotation.Quantity = supplierQuotationDto.SupplierQuantity;
            quotation.FinalPrice = supplierQuotationDto.FinalPrice.ToString();
            quotation.UpdatedAt = DateTime.UtcNow;

            _context.SupplierQuotations.Update(quotation);
            await _context.SaveChangesAsync();
            return new ResponseModel { Success = true, Message = "Supplier Quotation Updated Successfully" };
        }

        public async Task<ResponseModel> DeleteSupplierQuotation(int quotationId)
        {
            var quotation = _context.SupplierQuotations.Where(sq => sq.Deleted == 0 && sq.Id == quotationId).FirstOrDefault();
            if (quotation == null)
            {
                return new ResponseModel { Success = false, Message = "No Supplier Quotation Found" };
            }

            quotation.Deleted = 1;
            _context.SupplierQuotations.Update(quotation);
            await _context.SaveChangesAsync();
            return new ResponseModel { Success = true, Message = "Supplier Quotation Deleted Successfully" };
        }

        private int ParsePrice(string? price)
        {
            if (int.TryParse(price, out int parsedPrice))
            {
                return parsedPrice;
            }
            return 0;
        }
    }
}
