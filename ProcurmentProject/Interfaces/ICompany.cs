using ProcurmentProject.Dto;
using ProcurmentProject.Models;

namespace ProcurmentProject.Interfaces
{
    public interface ICompany
    {
        public Task<(bool success, string message)> AddCompany(CompanyDto company);
        public Task<List<Object>> GetAllCompany();
    }
}
