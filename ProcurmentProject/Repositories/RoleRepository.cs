using Microsoft.EntityFrameworkCore;
using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;

namespace ProcurmentProject.Repositories
{
    public class RoleRepository : IRole
    {
        private readonly ProcurmentSystemContext _context;
        public RoleRepository(ProcurmentSystemContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string message)> AddAccessRole(RoleDto role)
        {
            if(role.RoleName == null || role.Permission == null)
            {
                return (false, "Both the field are required");
            }

            var accessRole = new Role
            {
                Name = role.RoleName,
                Permission = role.Permission,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Roles.Add(accessRole);
            await _context.SaveChangesAsync();

            return (true, "Successfully added role");
        }
        public async Task<(bool success, string message, Role? role)> GetRole(int roleId)
        {
            if (roleId == null)
            {
                return (false, "Role Id is required", null);
            }

            var role =  _context.Roles.FirstOrDefault(role => role.Id == roleId);
            if(role == null)
            {
                return (false, "Id is Invalid", null);
            }
            else
            {
                return (true, "Role Fetch Successfully", role);
            }
        }

        public async  Task<Object> GetAllUserRole()
        {
            var userRole = _context.Users
                .Include(ur => ur.UserRoles)
                    .ThenInclude(r => r.Role)
                .Where(r => r.Deleted == 0)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Phone,
                    roleId = u.UserRoles.Select(ur => ur.Role.Id).FirstOrDefault(),
                    roleName = u.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault(),
                    permission = u.UserRoles.Select(ur => ur.Role.Permission).FirstOrDefault(),
                })
                .Where(r => r.roleName != null && r.permission != null);
                

            return userRole.Cast<object>();
        }

        public async Task<(bool success, string message)> DeleteRole(int roleId)
        {
            
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return (false, "Invalid ID");
            }
            role.Deleted = 1;
            await _context.SaveChangesAsync();
            return (true, "Remove Successfully");
            

        }
        public async Task<(bool success, string message)> UpdateRole(int id, RoleDto role)
        {
            var accessRole = await _context.Roles.FindAsync(id);
            if (accessRole == null)
            {
                return (false, "the id is invalid");
            }
            accessRole.UpdatedAt = DateTime.Now;
            accessRole.Name = role.RoleName;
            accessRole.Permission = role.Permission;

            await _context.SaveChangesAsync();
            return (true, "Data Updated Successfully");
        }

        public async Task<(bool success,string message, string? permission)> GetPermissionByUserId(int userId)
        {
            var userPermission = _context.UserRoles
                .Where(u => u.UserId == userId)
                .Select(ur => new { permission = ur.Role.Permission})
                .FirstOrDefault();
            if(userPermission == null)
            {
                return (false,"Something wents wrong",null);
            }
            return (true,"permission fetch successfully",userPermission.permission);
        }
    }
}
