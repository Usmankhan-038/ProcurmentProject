using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using ProcurmentProject.Dto;
using ProcurmentProject.Models;

namespace ProcurmentProject.Interfaces
{
    public interface IRole
    {
        public Task<(bool success, string message)> AddAccessRole(RoleDto role);
        public Task<(bool success, string message, Role? role)> GetRole(int roleId);
        public Task<Object> GetAllUserRole();
        public Task<(bool success, string message)> DeleteRole(int roleId);
        public Task<(bool success, string message)> UpdateRole(int id,RoleDto role);
        public Task<(bool success, string message, string? permission)> GetPermissionByUserId(int userId);

    }
}
