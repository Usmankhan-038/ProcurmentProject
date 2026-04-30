using Microsoft.EntityFrameworkCore;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ProcurmentProject.Repositories
{
    public class RoleRepository : IRole
    {
        private readonly ProcurmentSystemContext _context;
        private readonly IMemoryCache _cache;
        public RoleRepository(ProcurmentSystemContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<ResponseModel> AddAccessRole(RoleDto role)
        {
            if(role.RoleName == null || role.Permission == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Both the field are required"
                };
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
            _cache.Remove("Roles");
            _cache.Remove("UserRoles");

            return new ResponseModel
            {
                Success = true,
                Message = "Successfully added role",
                Id = Convert.ToInt32(accessRole.Id)
            };
        }
        public async Task<ResponseModel> GetRole(int roleId)
        {
            var role =  _context.Roles.FirstOrDefault(role => role.Id == roleId);
            if(role == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Id is Invalid"
                };
            }
            else
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = "Role Fetch Successfully",
                    Data = role
                };
            }
        }

        public async Task<ResponseModel> GetAllUserRole()
        {
            var cacheKey = "UserRoles";
            if (!_cache.TryGetValue(cacheKey, out dynamic? userRole))
            {
                userRole = await _context.Users
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
                    .Where(r => r.roleName != null && r.permission != null)
                    .ToListAsync();
                _cache.Set(cacheKey, (object)userRole, TimeSpan.FromHours(1));
            }

            if (userRole?.Count == 0)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "No User Role Found"
                };
            }

            return new ResponseModel
            {
                Success = true,
                Message = "User Role Fetch Successfully",
                Data = userRole
            };
        }

        public async Task<ResponseModel> DeleteRole(int roleId)
        {
            
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Invalid ID"
                };
            }
            role.Deleted = 1;
            await _context.SaveChangesAsync();
            _cache.Remove("Roles");
            _cache.Remove("UserRoles");
            return new ResponseModel
            {
                Success = true,
                Message = "Remove Successfully"
            };
            

        }
        public async Task<ResponseModel> UpdateRole(int id, RoleDto role)
        {
            var accessRole = await _context.Roles.FindAsync(id);
            if (accessRole == null)
            {
                return new ResponseModel { 
                    Success = false, 
                    Message = "the id is invalid" 
                };
            }
            accessRole.UpdatedAt = DateTime.Now;
            accessRole.Name = role.RoleName;
            accessRole.Permission = role.Permission;

            await _context.SaveChangesAsync();
            _cache.Remove("Roles");
            _cache.Remove("UserRoles");
            return new ResponseModel
            {
               Success =  true, 
                Message = "Data Updated Successfully",
                
            };
        }

        public async Task<ResponseModel> GetPermissionByUserId(int userId)
        {
            var userPermission = _context.UserRoles
                .Where(u => u.UserId == userId)
                .Select(ur => new { permission = ur.Role.Permission})
                .FirstOrDefault();
            if(userPermission == null)
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Something wents wrong"
                };
            }
            return new ResponseModel
            {
                Success = true,
                Message = "permission fetch successfully",
                Data = userPermission.permission
            };
        }

    }
}
