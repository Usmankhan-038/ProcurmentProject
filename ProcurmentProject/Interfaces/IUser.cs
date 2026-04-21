using ProcurmentProject.Dto;
using ProcurmentProject.Models;
namespace ProcurmentProject.Interfaces
{
    public interface IUser
    {
        public Task<(bool success, string message)> CreateUser(SignUpDto user);
        public Task<(bool success, string? token, string message)> Login(string userEmail, string password);
        public string GenerateAccessToken(int id, string role);

    }
}
