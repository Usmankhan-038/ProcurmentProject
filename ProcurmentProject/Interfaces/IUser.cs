using ProcurmentProject.Dto;
using ProcurmentProject.Data.Models;
namespace ProcurmentProject.Interfaces
{
    public interface IUser
    {
        public Task<ResponseModel> CreateUser(SignUpDto user);
        public Task<ResponseModel> Login(string userEmail, string password);
        public string GenerateAccessToken(int id, int roleId, string role);

    }
}
