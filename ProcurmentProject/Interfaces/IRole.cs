using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using ProcurmentProject.Dto;
using ProcurmentProject.Models;

namespace ProcurmentProject.Interfaces
{
    public interface IRole
    {
        public Task<ResponseModel> AddAccessRole(RoleDto role);
        public Task<ResponseModel> GetRole(int roleId);
        public Task<ResponseModel> GetAllUserRole();
        public Task<ResponseModel> DeleteRole(int roleId);
        public Task<ResponseModel> UpdateRole(int id,RoleDto role);
        public Task<ResponseModel> GetPermissionByUserId(int userId);

    }
}
