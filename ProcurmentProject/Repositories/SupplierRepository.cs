using Microsoft.EntityFrameworkCore;
//using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ProcurmentProject.Repositories
{
    public class SupplierRepository: ISupplier
    {
        private readonly ProcurmentSystemContext _context;
        private readonly IMemoryCache _cache;
        public SupplierRepository(ProcurmentSystemContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<ResponseModel> AddSupplier(SuppliersDto supplier)
        {
            if (supplier == null)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Correct data" };
            }
            var newUser = new User
            {
                Name = supplier.UserData.Name,
                Email = supplier.UserData.Email,
                Phone = supplier.UserData.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(supplier.UserData.Password),

            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            _cache.Remove(supplier);
            var supplierRoleId = _context.Roles.Where(p => p.Name == "Supplier").Select(pp => new { id = pp.Id }).FirstOrDefault();
            if (supplierRoleId == null)
            {
                return new ResponseModel { Success = false, Message = "Something wents wrong" };
            }
            var newUserRole = new UserRole
            {
                UserId = newUser.Id,
                RoleId = supplierRoleId.id
            };
            _context.UserRoles.Add(newUserRole);
            var newSupplier = new Supplier
            {
                UserId = newUser.Id,
                CompanyName = supplier.CompanyName,
                NtnTaxNumber = supplier.NtnTaxNumber,
            };
            _context.Suppliers.Add(newSupplier);
            await _context.SaveChangesAsync();
            _cache.Remove(supplier);
            return new ResponseModel
            {
                Success = true,
                Message = "Supplier Added Successfully",
                Id = newSupplier.Id
            };
        }
        public async Task<ResponseModel> UpdateSupplier(int Id, SuppliersDto supplierDto)
        {
            var supplier = _context.Suppliers.Where(s=>s.Deleted==0 && s.Id == Id).FirstOrDefault();
            if (supplierDto == null)
            {
                return new ResponseModel { Success = false, Message = "Please Provide Correct data" };
            }
            if (supplier == null)
            {
                return new ResponseModel { Success = false, Message = "Please Enter Correct Id" };
            }
            var supplierUser = _context.Users.Where(s => s.Deleted==0 && s.Id == supplier.UserId).FirstOrDefault();
            if (supplierUser == null)
            {
                return new ResponseModel { Success = false, Message = "Something wents wrong" };
            }

            supplierUser.Name = supplierDto.UserData.Name;
            supplierUser.Email = supplierDto.UserData.Email;
            supplierUser.Phone = supplierDto.UserData.Phone;
            supplierUser.Password = BCrypt.Net.BCrypt.HashPassword(supplierDto.UserData.Password);

            supplier.CompanyName = supplierDto.CompanyName;
            supplier.NtnTaxNumber = supplierDto.NtnTaxNumber;

            await _context.SaveChangesAsync();
            _cache.Remove(supplier);
            return new ResponseModel { Success = true, Message = "Supplier Update Successfully" };
        }
        
        public async Task<ResponseModel> DeleteSupplier(int supplierId)
        {
            var supplier =  _context.Suppliers.Where(s => s.Deleted == 0 && s.Id == supplierId).FirstOrDefault();
            
            if (supplier == null)
            {
                return new ResponseModel { Success = false, Message = "Please Enter Correct Id" };
            }

            var supplierUser = _context.Users.Where(s => s.Deleted == 0 && s.Id == supplier!.UserId).FirstOrDefault();
            
            if (supplierUser == null)
            {
                return new ResponseModel { Success = false, Message = "Something wents wrong" };
            }
            supplierUser.Deleted = 1;
            supplier.Deleted = 1;

            _cache.Remove(supplier);
            await _context.SaveChangesAsync();
            return new ResponseModel { Success = true, Message = "Supplier Deleted Successfully" };
        }
        public async Task<ResponseModel> GetAllSupplier()
        {
            var cacheKey = "Supplier";

            if (!_cache.TryGetValue(cacheKey, out var suppliers))
            {
                  suppliers = await _context.Suppliers
                 .Where(s => s.Deleted == 0)
                 .Select(supplier => new
                 {
                     supplierId = supplier.Id,
                     Name = supplier.User.Name,
                     Email = supplier.User.Email,
                     Phone = supplier.User.Phone,
                     CompanyName = supplier.CompanyName,
                     NtnNumber = supplier.NtnTaxNumber,
                 }).ToListAsync();
                _cache.Set(cacheKey, suppliers, TimeSpan.FromHours(1));

            }
         
            if(suppliers == null)
            {
                return new ResponseModel { Success = false, Message = "No Data Found" };
            }
            return new ResponseModel
            {
                Success = true,
                Message = "All Data fetch successfully",
                Data = suppliers
            };
        }
    }
}
