using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Helper;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;
using System.Data;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Dapper;

namespace ProcurmentProject.Repositories
{
    public class RequestForQuotationRepository : IRequestForQuotation
    {
        private readonly ProcurmentSystemContext _context;
        private readonly IEmailService _email;
        private readonly IConfiguration _config;
        private readonly DocumentUploader _documentUploader;
        public RequestForQuotationRepository(ProcurmentSystemContext context, IEmailService email, IConfiguration config, DocumentUploader documentUploader)
        {
            _context = context;
            _email = email;
            _config = config;
            _documentUploader = documentUploader;
        }
        public async Task<ResponseModel> CreateRfq(int PrId, RfqDto rfqDto)
        {
            if(PrId == 0)
            {
                return new ResponseModel { Success = false, Message = "please enter correct Pr Id" };
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
                try
                {
                    var document = await _documentUploader.UploadDocument(newRfq.Id, "rfq", rfqDto.Attachment);
                    _context.Documents.Add(document);
                    await _context.SaveChangesAsync();
                }
                catch (InvalidDataException ex)
                {
                    return new ResponseModel { Success = false, Message = ex.Message };
                }
            }

            return new ResponseModel { Success = true, Message = "RFQ Created Successful", Id = newRfq.Id };
        }
        public async Task<ResponseModel> SendQuotationToAllSupplier(int rfqId)
        { 
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();

            if (rfq == null)
            {
                return new ResponseModel { Success = false, Message = "please enter correct id" };
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
            return new ResponseModel { Success = true, Message = "Send Quotation to All Supplier" };
        }
        public async Task<ResponseModel> SendQuotationToSpecificSupplier(List<int> supplierId, int rfqId)
        {
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();

            if (rfq == null)
            {
                return new ResponseModel { Success = false, Message = "please enter correct id" };
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
            return new ResponseModel { Success = true, Message = "Send Quotation to All Supplier" };
        }

        public async Task<ResponseModel> GetAllRfqs()
        {
            var baseUrl = _config.GetConnectionString("BaseUrl");

            //var rfqs = await _context.Database.SqlQuery<RfqDetailDto>($"EXEC sp_GetAllRfq").ToListAsync();

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var rfqs = (await connection.QueryAsync<RfqDetailDto>(
                    "sp_GetAllRfq",
                    commandType: CommandType.StoredProcedure
                    )).ToList();
                if (rfqs.Count == 0)
                {
                    return new ResponseModel { Success = false, Message = "No RFQ Found" };
                }
                return new ResponseModel
                {
                    Success = true,
                    Message = "Successfully Fetch Rfqs",
                    Data = rfqs
                };
            }
            
        }
        public async Task<ResponseModel> UpdateRfq(int rfqId, RfqDto rfqDto)
        {
            if (rfqId == 0)
            {
                return new ResponseModel { Success = false, Message = "please enter correct  Rfq Id" };
            }
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();

            if (rfq == null)
            {
                return new ResponseModel { Success = false, Message = "Please Enter Correct Id" };
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
                try
                {
                    var newDocument = await _documentUploader.UploadDocument(rfqId, "rfq", rfqDto.Attachment);
                    _context.Documents.Add(newDocument);
                    await _context.SaveChangesAsync();
                }
                catch (InvalidDataException ex)
                {
                    return new ResponseModel { Success = false, Message = ex.Message };
                }
            }
            return new ResponseModel { Success = true, Message = "Rfq updated Successfully" };
        }
        public async Task<ResponseModel> DeleteRfq(int rfqId)
        {
            
            if(rfqId == 0)
            {
                return new ResponseModel { Success = false, Message = "Please Enter correct Id" };
            }
            var rfq = _context.Rfqs.Where(r => r.Deleted == 0 && r.Id == rfqId).FirstOrDefault();
            if (rfq == null)
            {
                return new ResponseModel { Success = false, Message = "Please Enter Correct Id" };
            }
            rfq.Deleted = 1; 
            await _context.SaveChangesAsync();
            return new ResponseModel { Success = true, Message = "Rfq Deleted SuccessFully" };
        }
    }
}
