using Microsoft.EntityFrameworkCore;
using ProcurmentProject.Data;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;

namespace ProcurmentProject.Repositories
{
    public class SupplierRepository: ISupplier
    {
        private readonly ProcurmentSystemContext _context;
        public SupplierRepository(ProcurmentSystemContext context)
        {
            _context = context;
        }
        public async Task<(bool success, string message)> AddSupplier(SuppliersDto supplier)
        {
            if (supplier == null)
            {
                return (false, "Please Provide Correct data");
            }
            var newUser = new User
            {
                Name = supplier.userData.name,
                Email = supplier.userData.email,
                Phone = supplier.userData.phone,
                Password = BCrypt.Net.BCrypt.HashPassword(supplier.userData.password),

            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            var supplierRoleId = _context.Roles.Where(p => p.Name == "Supplier").Select(pp => new { id = pp.Id }).FirstOrDefault();
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
            return (true, "Supplier Added Successfully");
        }
        public async Task<(bool success, string message)> UpdateSupplier(int Id, SuppliersDto supplierDto)
        {
            var supplier = _context.Suppliers.Where(s=>s.Deleted==0 && s.Id == Id).FirstOrDefault();
            var supplierUser = _context.Users.Where(s => s.Deleted==0 && s.Id == supplier.UserId).FirstOrDefault();
            if (supplierDto == null)
            {
                return (false, "Please Provide Correct data");
            }

            supplierUser.Name = supplierDto.userData.name;
            supplierUser.Email = supplierDto.userData.email;
            supplierUser.Phone = supplierDto.userData.phone;
            supplierUser.Password = BCrypt.Net.BCrypt.HashPassword(supplierDto.userData.password);


            supplier.CompanyName = supplierDto.CompanyName;
            supplier.NtnTaxNumber = supplierDto.NtnTaxNumber;
            await _context.SaveChangesAsync();
            return (true, "Supplier Update Successfully");
        }
        
        public async Task<(bool success, string message)> DeleteSupplier(int supplierId)
        {
            var supplier = _context.Suppliers.Where(s => s.Deleted == 0 && s.Id == supplierId).FirstOrDefault();
            var supplierUser = _context.Users.Where(s => s.Deleted == 0 && s.Id == supplier.UserId).FirstOrDefault();
            if (supplier == null)
            {
                return (false, "Please Enter Correct Id");
            }
            else if (supplierUser == null)
            {
                return (false, "Something wents wrong");
            }
            supplierUser.Deleted = 1;
            supplier.Deleted = 1;
            await _context.SaveChangesAsync();
            return (true, "Supplier Deleted Successfully");
        }
        public async Task<(bool success, string message, Object? supplier)> GetAllSupplier()
        {
            var suppliers = await _context.Suppliers
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
            if(suppliers == null)
            {
                return (true, "No Data Found", null);
            }
            return (true, "All Data fetch successfully", suppliers);
        }
    }
}
