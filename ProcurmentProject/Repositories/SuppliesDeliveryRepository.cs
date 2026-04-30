using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
//using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Helper;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Data.Models;

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

        public async Task<ResponseModel> AddSuppliesDelivery(int rfqId, int supplierId, SupplierDeliveryDto supplierDeliveryDto)
        {
            if (supplierDeliveryDto == null)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Valid Data" };
            }

            if (rfqId == 0 || supplierId == 0)
            {
                return new ResponseModel { Success = false, Message = "Please Enter Correct Id" };
            }

            var supplier = _context.Suppliers.Where(s => s.Deleted == 0 && s.Id == supplierId).FirstOrDefault();
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();
            if (supplier == null || rfq == null)
            {
                return new ResponseModel { Success = false, Message = "Please Enter Correct Id" };
            }

            var isParsed = DateTime.TryParse(supplierDeliveryDto.RecivingDateTime, out DateTime receivingDateTime);
            if (!isParsed)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Correct Reciving Date Time" };
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
                try
                {
                    var belongingName = "supplier_delivery";
                    var document = await _doc.UploadDocument(suppliesDelivery.Id, belongingName,supplierDeliveryDto.Attachment);
                    _context.Documents.Add(document);
                    await _context.SaveChangesAsync();
                }
                catch (InvalidDataException ex)
                {
                    return new ResponseModel { Success = false, Message = ex.Message };
                }
            }

            return new ResponseModel { Success = true, Message = "Supplies Delivery Added Successfully", Id = suppliesDelivery.Id };
        }

        public async Task<ResponseModel> GetSuppliesDelivery(int? suppliesDeliveryId = null)
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
                        url = $"/uploads/{d.EncodedFileName}"
                    }).FirstOrDefault()
            }).ToListAsync();

            if (suppliesDelivery.Count == 0)
            {
                return new ResponseModel { Success = false, Message = "No Supplies Delivery Found" };
            }

            return new ResponseModel
            {
                Success = true,
                Message = "Supplies Delivery Fetch Successfully",
                Data = suppliesDelivery
            };
        }

        public async Task<ResponseModel> UpdateSuppliesDelivery(int suppliesDeliveryId, SupplierDeliveryDto supplierDeliveryDto)
        {
            if (supplierDeliveryDto == null)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Valid Data" };
            }

            var suppliesDelivery = _context.SuppliesDeliveries.Where(sd => sd.Deleted == 0 && sd.Id == suppliesDeliveryId).FirstOrDefault();
            if (suppliesDelivery == null)
            {
                return new ResponseModel { Success = false, Message = "No Supplies Delivery Found" };
            }

            var isParsed = DateTime.TryParse(supplierDeliveryDto.RecivingDateTime, out DateTime receivingDateTime);
            if (!isParsed)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Correct Reciving Date Time" };
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
                try
                {
                    var belongingName = "supplier_delivery";
                    var newDocument = await _doc.UploadDocument(suppliesDeliveryId, belongingName, supplierDeliveryDto.Attachment);
                    _context.Documents.Add(newDocument);
                    await _context.SaveChangesAsync();
                }
                catch (InvalidDataException ex)
                {
                    return new ResponseModel { Success = false, Message = ex.Message };
                }
            }

            return new ResponseModel { Success = true, Message = "Supplies Delivery Updated Successfully" };
        }

        public async Task<ResponseModel> GetSupplierDeliveryView()
        {
            var deliveries = await _context.SupplierDeliveryViews
                .AsNoTracking()
                .Select(view => new SupplierDeliveryViewDto
                {
                    Title = view.Title,
                    RfqStatus = view.RfqStatus,
                    UnitPrice = view.UnitPrice,
                    Quantity = view.Quantity,
                    FinalPrice = view.FinalPrice,
                    RecivedByName = view.RecivedByName,
                    DeliveryStatus = view.DeliveryStatus,
                    DeliveryNote = view.DeliveryNote,
                    RecevingDatetime = view.RecevingDatetime
                })
                .ToListAsync();

            if (deliveries.Count == 0)
            {
                return new ResponseModel { Success = false, Message = "No Supplier Delivery Found" };
            }

            return new ResponseModel
            {
                Success = true,
                Message = "Supplier Delivery Fetch Successfully",
                Data = deliveries
            };
        }

        public async Task<ResponseModel> DeleteSuppliesDelivery(int suppliesDeliveryId)
        {
            var suppliesDelivery = _context.SuppliesDeliveries.Where(sd => sd.Deleted == 0 && sd.Id == suppliesDeliveryId).FirstOrDefault();
            if (suppliesDelivery == null)
            {
                return new ResponseModel { Success = false, Message = "No Supplies Delivery Found" };
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
            return new ResponseModel { Success = true, Message = "Supplies Delivery Deleted Successfully" };
        }

        
    }
}
