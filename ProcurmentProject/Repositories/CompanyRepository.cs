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

        public async Task<(bool success,string message)> addCompany(CompanyDto company)
        {
            if (company == null)
            {
                return (false, "Please Enter the detail correctly");
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

            return (true, "Company Add Successfully");
        }
        public async Task<List<Object>> getAllCompany()
        {
            var companies = await _context.Companies.Where(company => company.Deleted == 0).Select(company => new { company.Id, company.Name}).ToListAsync();

            return companies.Cast<object>().ToList();
        }

    }
}
