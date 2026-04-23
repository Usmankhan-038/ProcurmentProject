using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Helper;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;

namespace ProcurmentProject.Repositories
{
    public class SuppliesDeliveryRepository : ISuppliesDelivery
    {
        private readonly ProcurmentSystemContext _context;
        private readonly DocumentUploader _doc;
        public SuppliesDeliveryRepository(ProcurmentSystemContext context, DocumentUploader doc)
        {
            _context = context;
            _doc = doc;
        }

        public async Task<(bool success, string message)> AddSuppliesDelivery(int rfqId, int supplierId, SupplierDeliveryDto supplierDeliveryDto)
        {
            if (supplierDeliveryDto == null)
            {
                return (false, "Please Provide Valid Data");
            }

            if (rfqId == 0 || supplierId == 0)
            {
                return (false, "Please Enter Correct Id");
            }

            var supplier = _context.Suppliers.Where(s => s.Deleted == 0 && s.Id == supplierId).FirstOrDefault();
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();
            if (supplier == null || rfq == null)
            {
                return (false, "Please Enter Correct Id");
            }

            var isParsed = DateTime.TryParse(supplierDeliveryDto.RecivingDateTime, out DateTime receivingDateTime);
            if (!isParsed)
            {
                return (false, "Please Provide Correct Reciving Date Time");
            }

            var suppliesDelivery = new SuppliesDelivery
            {
                RfqId = rfqId,
                SuplierId = supplierId,
                RecevingDatetime = receivingDateTime,
                RecivedBy = supplierDeliveryDto.RecivedBy,
                Status = supplierDeliveryDto.Status,
                Note = supplierDeliveryDto.Note,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SuppliesDeliveries.Add(suppliesDelivery);
            await _context.SaveChangesAsync();

            if (supplierDeliveryDto.Attachment != null)
            {
                var belongingName = "supplier_delivery";
                var document = await _doc.UploadDocument(suppliesDelivery.Id, belongingName,supplierDeliveryDto.Attachment);
                _context.Documents.Add(document);
                await _context.SaveChangesAsync();
            }

            return (true, "Supplies Delivery Added Successfully");
        }

        public async Task<(bool success, string message, object? suppliesDelivery)> GetSuppliesDelivery(int? suppliesDeliveryId = null)
        {
            var query = _context.SuppliesDeliveries.Where(sd => sd.Deleted == 0);
            if (suppliesDeliveryId.HasValue)
            {
                query = query.Where(sd => sd.Id == suppliesDeliveryId.Value);
            }

            var suppliesDelivery = await query.Select(sd => new
            {
                deliveryId = sd.Id,
                rfqId = sd.RfqId,
                prTitle = sd.Rfq!.Pr!.Title,
                supplierId = sd.SuplierId,
                supplierName = sd.Suplier!.User.Name,
                companyName = sd.Suplier.CompanyName,
                recivingDateTime = sd.RecevingDatetime,
                recivedBy = sd.RecivedBy,
                status = sd.Status,
                note = sd.Note,
                document = _context.Documents.Where(d => d.Deleted == 0 && d.BelongName == "supplier_delivery" && d.BelongId == sd.Id)
                    .Select(d => new
                    {
                        documentId = d.Id,
                        fileName = d.OriginalFileName,
                        encodedFileName = d.EncodedFileName,
                        url = d.Url
                    }).FirstOrDefault()
            }).ToListAsync();

            if (suppliesDelivery.Count == 0)
            {
                return (false, "No Supplies Delivery Found", null);
            }

            return (true, "Supplies Delivery Fetch Successfully", suppliesDelivery);
        }

        public async Task<(bool success, string message)> UpdateSuppliesDelivery(int suppliesDeliveryId, SupplierDeliveryDto supplierDeliveryDto)
        {
            if (supplierDeliveryDto == null)
            {
                return (false, "Please Provide Valid Data");
            }

            var suppliesDelivery = _context.SuppliesDeliveries.Where(sd => sd.Deleted == 0 && sd.Id == suppliesDeliveryId).FirstOrDefault();
            if (suppliesDelivery == null)
            {
                return (false, "No Supplies Delivery Found");
            }

            var isParsed = DateTime.TryParse(supplierDeliveryDto.RecivingDateTime, out DateTime receivingDateTime);
            if (!isParsed)
            {
                return (false, "Please Provide Correct Reciving Date Time");
            }

            suppliesDelivery.RecevingDatetime = receivingDateTime;
            suppliesDelivery.RecivedBy = supplierDeliveryDto.RecivedBy;
            suppliesDelivery.Status = supplierDeliveryDto.Status;
            suppliesDelivery.Note = supplierDeliveryDto.Note;
            suppliesDelivery.UpdatedAt = DateTime.UtcNow;

            _context.SuppliesDeliveries.Update(suppliesDelivery);
            await _context.SaveChangesAsync();

            if (supplierDeliveryDto.Attachment != null)
            {
                var documents = await _context.Documents
                    .Where(d => d.Deleted == 0 && d.BelongName == "supplier_delivery" && d.BelongId == suppliesDeliveryId)
                    .ToListAsync();

                foreach (var document in documents)
                {
                    document.Deleted = 1;
                }

                await _context.SaveChangesAsync();
                var belongingName = "supplier_delivery";
                var newDocument = await _doc.UploadDocument(suppliesDeliveryId, belongingName, supplierDeliveryDto.Attachment);
                _context.Documents.Add(newDocument);
                await _context.SaveChangesAsync();
            }

            return (true, "Supplies Delivery Updated Successfully");
        }

        public async Task<(bool success, string message)> DeleteSuppliesDelivery(int suppliesDeliveryId)
        {
            var suppliesDelivery = _context.SuppliesDeliveries.Where(sd => sd.Deleted == 0 && sd.Id == suppliesDeliveryId).FirstOrDefault();
            if (suppliesDelivery == null)
            {
                return (false, "No Supplies Delivery Found");
            }

            suppliesDelivery.Deleted = 1;

            var documents = await _context.Documents
                .Where(d => d.Deleted == 0 && d.BelongName == "supplier_delivery" && d.BelongId == suppliesDeliveryId)
                .ToListAsync();

            foreach (var document in documents)
            {
                document.Deleted = 1;
            }

            await _context.SaveChangesAsync();
            return (true, "Supplies Delivery Deleted Successfully");
        }

        
    }
}
