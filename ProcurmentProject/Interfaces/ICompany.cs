using ProcurmentProject.Dto;
using ProcurmentProject.Data.Models;

namespace ProcurmentProject.Interfaces
{
    public interface ICompany
    {
        public Task<ResponseModel> AddCompany(CompanyDto company);
        public Task<ResponseModel> GetAllCompany();
    }
}
