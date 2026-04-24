using ProcurmentProject.Dto;
using ProcurmentProject.Models;

namespace ProcurmentProject.Interfaces
{
    public interface ICompany
    {
        public Task<ResponseModel> AddCompany(CompanyDto company);
        public Task<ResponseModel> GetAllCompany();
    }
}
