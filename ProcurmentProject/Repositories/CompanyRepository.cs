using ProcurmentProject.Models;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ProcurmentProject.Repositories
{
    public class CompanyRepository : ICompany
    {
        private readonly ProcurmentSystemContext _context;
        private readonly IMemoryCache _cache;
        public CompanyRepository(ProcurmentSystemContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
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
            _cache.Remove("Companies");

            return new ResponseModel { 
                Success = true, 
                Message = "Company Add Successfully" 
            };
        }
        public async Task<ResponseModel> GetAllCompany()
        {
            var cacheKey = "Companies";
            if (!_cache.TryGetValue(cacheKey, out dynamic? companies))
            {
                companies = await _context.Companies.Where(company => company.Deleted == 0).Select(company => new { company.Id, company.Name}).ToListAsync();
                _cache.Set(cacheKey, (object)companies, TimeSpan.FromHours(1));
            }
            if (companies?.Count == 0)
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
