using ProcurmentProject.Data;
using ProcurmentProject.Models;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProcurmentProject.Repositories
{
    public class CompanyRepository : ICompany
    {
        private readonly ProcurmentSystemContext _context;
        public CompanyRepository(ProcurmentSystemContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> AddCompany(CompanyDto company)
        {
            if (company == null)
            {
                return new ResponseModel {
                    Success =  false, 
                    Message = "Please Enter the detail correctly" 
                };
            }

            var newCompany = new Company
            {
                Name = company.CompanyName,
                NtnNumber = company.NTNNumber,
                RegisterIn = company.RegisterIn,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _context.Companies.Add(newCompany);
            await _context.SaveChangesAsync();

            return new ResponseModel { 
                Success = true, 
                Message = "Company Add Successfully" 
            };
        }
        public async Task<ResponseModel> GetAllCompany()
        {
            var companies = await _context.Companies.Where(company => company.Deleted == 0).Select(company => new { company.Id, company.Name}).ToListAsync();
            if (companies.Count == 0)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "No Company Found"
                };
            }

            return new ResponseModel
            {
                Success = true,
                Message = "Company Fetch Successfully",
                Data = companies
            };
        }

    }
}
