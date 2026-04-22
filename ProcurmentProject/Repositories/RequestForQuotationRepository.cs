using Microsoft.EntityFrameworkCore;
using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;

namespace ProcurmentProject.Repositories
{
    public class RequestForQuotationRepository : IRequestForQuotation
    {
        private readonly ProcurmentSystemContext _context;
        private readonly IEmailService _email;
        private readonly IConfiguration _config;
        public RequestForQuotationRepository(ProcurmentSystemContext context, IEmailService email, IConfiguration config)
        {
            _context = context;
            _email = email;
            _config = config;
        }
        public async Task<(bool success, string message)> CreateRfq(int PrId, RfqDto rfqDto)
        {
            if(PrId == null)
            {
                return (false, "please enter correct Pr Id");
            }

            var newRfq = new Rfq
            {
                Status = rfqDto.Status,
                PrId = PrId,
            };
            _context.Rfqs.Add(newRfq);
            await _context.SaveChangesAsync();
            if (rfqDto.Attachment != null)
            {

                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "upload", "rfqs");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var OrignalFileName = Path.GetFileName(rfqDto.Attachment.FileName);
                var extension = Path.GetExtension(rfqDto.Attachment.FileName);
                var encodedFileName = Guid.NewGuid().ToString() + extension;

                string fullPath = Path.Combine(folderPath, encodedFileName);

                using (var stream = new FileStream(fullPath,FileMode.Create))
                {
                    await rfqDto.Attachment.CopyToAsync(stream);
                }

                var newDocument = new Document
                {
                    BelongId = newRfq.Id,
                    BelongName = "rfq",
                    EncodedFileName = encodedFileName,
                    OriginalFileName = OrignalFileName,
                    Url = folderPath
                };
                _context.Documents.Add(newDocument);
                await _context.SaveChangesAsync();

            }

            return (true, "RFQ Created Successful");
        }
        public async Task<(bool success, string message)> SendQuotationToAllSupplier(int rfqId)
        { 
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();

            if (rfq == null)
            {
                return (false, "please enter correct id");
            }

            var suppliers =  _context.Suppliers.Where(s => s.Deleted == 0)
           .Select(ss => new {
               supplierId = ss.Id,
               Name = ss.User.Name,
               Email = ss.User.Email,
               Phone = ss.User.Phone,
               CompanyName = ss.CompanyName,
               NtnNumber = ss.NtnTaxNumber

            }).ToList();
            foreach (var sup in suppliers)
            {
                if (sup.Email != null)
                {
                    var port = "7074";
                    var messageBody = $@"
                    <div style=""font-family: 'Segoe UI', Arial, sans-serif; color: #202124; line-height: 1.6;"">
                        <p>Dear <strong>{sup.CompanyName}</strong>,</p>

                        <p>This email confirms that we have received your registration request for the Procurement Portal.</p>

                        <p><strong>Application Details:</strong><br>
                        -----------------------------------------<br>
                        <strong>NTN Number:</strong> {sup.NtnNumber}<br>
                        <strong>Status:</strong> Pending Verification<br>
                        -----------------------------------------</p>

                        <p>Please click the button below to share the price of the quotation:</p>

                        <div style=""margin: 30px 0;"">
                            <a href=""https://localhost:{port}/swagger/index.html?supplierId={sup.supplierId}&rfqId={rfqId}"" 
                               style=""background-color: #1a73e8; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-weight: bold; display: inline-block;"">
                               Submit Price Quotation
                            </a>
                        </div>

                        <p>If you have any questions, please contact our support desk.</p>

                        <br>
                        <p>Regards,</p>
                        <p><strong>Procurement Department</strong><br>
                        Islamabad, Pakistan</p>
    
                        <p style=""font-size: 11px; color: #70757a; margin-top: 30px;"">
                            Note: This is a system-generated email. Please do not reply.
                        </p>
                    </div>";
                    var subject = $"Quotation For RFQ RFQ-{rfq.Id}";
                    await _email.SendMail(sup.Email!, subject, messageBody);
                }
            }
            return (true, "Send Quotation to All Supplier");
        }
        public async Task<(bool success, string message)> SendQuotationToSpecificSupplier(List<int> supplierId, int rfqId)
        {
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();

            if (rfq == null)
            {
                return (false, "please enter correct id");
            }

            var suppliers = _context.Suppliers.Where(s => s.Deleted == 0 && supplierId.Contains(s.Id))
           .Select(ss => new
           {
               supplierId = ss.Id,
               Name = ss.User.Name,
               Email = ss.User.Email,
               Phone = ss.User.Phone,
               CompanyName = ss.CompanyName,
               NtnNumber = ss.NtnTaxNumber

           }).ToList();
            foreach (var sup in suppliers)
            {
                if(sup.Email != null)
                {
                    var port = "7074";
                    var messageBody = $@"
                    <div style=""font-family: 'Segoe UI', Arial, sans-serif; color: #202124; line-height: 1.6;"">
                        <p>Dear <strong>{sup.CompanyName}</strong>,</p>

                        <p>This email confirms that we have received your registration request for the Procurement Portal.</p>

                        <p><strong>Application Details:</strong><br>
                        -----------------------------------------<br>
                        <strong>NTN Number:</strong> {sup.NtnNumber}<br>
                        <strong>Status:</strong> Pending Verification<br>
                        -----------------------------------------</p>

                        <p>Please click the button below to share the price of the quotation:</p>

                        <div style=""margin: 30px 0;"">
                            <a href=""https://localhost:{port}/swagger/index.html?supplierId={sup.supplierId}&rfqId={rfqId}"" 
                               style=""background-color: #1a73e8; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-weight: bold; display: inline-block;"">
                               Submit Price Quotation
                            </a>
                        </div>

                        <p>If you have any questions, please contact our support desk.</p>

                        <br>
                        <p>Regards,</p>
                        <p><strong>Procurement Department</strong><br>
                        Islamabad, Pakistan</p>
    
                        <p style=""font-size: 11px; color: #70757a; margin-top: 30px;"">
                            Note: This is a system-generated email. Please do not reply.
                        </p>
                    </div>";
                    var subject = $"Quotation For RFQ RFQ-{rfq.Id}";
                    await _email.SendMail(sup.Email!, subject, messageBody);
                }
             
            }
            return (true, "Send Quotation to All Supplier");
        }

        public async Task<(bool success, string message, object? rfqs)> GetAllRfqs()
        {
            var baseUrl = _config.GetConnectionString("BaseUrl");

            var rfqs = await _context.Rfqs.Where(r => r.Deleted == 0)
                .Join(_context.PurchasedRequisitions, r => r.PrId, p => p.Id, (r, p) => new { r, p })
                .Join(_context.PrProducts, combined => combined.p.Id, prProd => prProd.Id, (combined, prProd) => new {combined, prProd})
                .Join(_context.Products, prCombine => prCombine.prProd.ProductId, prod => prod.Id, (prCombine, prod) => new
                {
                    RfqId = prCombine.combined.r.Id,
                    RfqStatus = prCombine.combined.r.Status,
                    PrTitle = prCombine.combined.p.Title,
                    PrQuantity = prCombine.combined.p.Quantity,
                    PrEstimatedBudget = prCombine.combined.p.EstimatedBudget,
                    PrDeliveryDate = prCombine.combined.p.DeliveryDate,
                    PrNote = prCombine.combined.p.Note,
                    PrProducts = new 
                    {
                        ProductName = prod.Name,
                        ProductCompany = prod.Company,
                        ProductDescription = prod.Description,
                        ProductUPC = prod.Upc,
                    },
                    Documents = _context.Documents
                    .Where(d => d.BelongName == "rfq" && d.BelongId == prCombine.combined.r.Id)
                    .Select(d => new
                    {
                        d.Id,
                        DocumentUrl =  d.Url + d.EncodedFileName
                       
                    }).ToList()
                

                }).ToListAsync();
            if(rfqs == null )
            {
                return (false, "No RFQ Found", null);
            }
            return (true, "Successfully Fetch Rfqs",rfqs);
        }
        public async Task<(bool success, string message)> UpdateRfq(int rfqId, RfqDto rfqDto)
        {
            if (rfqId == null)
            {
                return (false, "please enter correct  Rfq Id");
            }
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();

            if (rfq == null)
            {
                return (false, "Please Enter Correct Id");
            }
            rfq.Status = rfqDto.Status;

            _context.Rfqs.Add(rfq);
            await _context.SaveChangesAsync();
            if (rfqDto.Attachment != null)
            {
                var document = _context.Documents.Where(d => d.Deleted == 0 && d.BelongId == rfqId).FirstOrDefault();
                if(document != null)
                {
                    document.Deleted = 1;
                    _context.Documents.Add(document);
                    await _context.SaveChangesAsync();
                }

                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "rfqs");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var OrignalFileName = Path.GetFileName(rfqDto.Attachment.FileName);
                var extension = Path.GetExtension(rfqDto.Attachment.FileName);
                var encodedFileName = Guid.NewGuid().ToString() + DateTime.Now + extension;
                string fullPath = Path.Combine(folderPath, encodedFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await rfqDto.Attachment.CopyToAsync(stream);
                }

                var newDocument = new Document
                {

                    BelongName = "rfq",
                    EncodedFileName = encodedFileName,
                    OriginalFileName = OrignalFileName,
                    Url = folderPath
                };
                _context.Documents.Add(newDocument);
                await _context.SaveChangesAsync();
            }
            return (true, "Rfq updated Successfully");
        }
        public async Task<(bool success, string message)> DeleteRfq(int rfqId)
        {
            
            if(rfqId == null)
            {
                return (false, "Please Enter correct Id");
            }
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();
            if (rfq == null)
            {
                return (false,"Please Enter Correct Id");
            }
            rfq.Deleted = 1; 
            await _context.SaveChangesAsync();
            return (true, "Rfq Deleted SuccessFully");
        }
    }
}
