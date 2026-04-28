using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProcurmentProject.Data;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;
using System.Transactions;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ProcurmentProject.Dto;
using Azure;

//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace ProcurmentProject.Repositories
{
    public class UserRepository : IUser
    {
        private readonly ProcurmentSystemContext _context;
        private readonly IConfiguration _config;
        public UserRepository(ProcurmentSystemContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<ResponseModel> CreateUser(SignUpDto signup)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (signup == null)
                {
                    return new ResponseModel { Success = false, Message = "The Data is incomplete. Please Enter Complete Data" };
                } else if (signup.Password != signup.Confirmpassword)
                {
                    return new ResponseModel { Success = false, Message = "Password and confirm password must be same" };
                }

                signup.Password = BCrypt.Net.BCrypt.HashPassword(signup.Password);

                bool isEmailExist = _context.Users.Any(user => user.Email == signup.Email);
                if (isEmailExist)
                {
                    return new ResponseModel { Success = false, Message = "This email is already exist,use another email please" };
                }
                var user = new User
                {
                    Name = signup.Name,
                    Email = signup.Email,
                    Phone = signup.Phone,
                    Password = signup.Password,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                

                _context.Users.Add(user);
                await _context.SaveChangesAsync();


                var company = _context.Companies.FirstOrDefault(user => user.Name == signup.CompanyName);

                if(company == null)
                {
                    return new ResponseModel { Success = false, Message = "Company you enter is not Valid" };
                }

                var userCompany = new UserCompany
                {
                    UserId = user.Id,
                    CompanyId = company!.Id,
                };

                _context.UserCompanies.Add(userCompany);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new ResponseModel { Success = true, Message = "User Created Successfully", Id = user.Id };
            } catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseModel { Success = false, Message = "Something went Wrong" };
            }
        }
        public async Task<ResponseModel> Login(string userEmail, string password)
        {
            if(userEmail == null && password == null)
            {
                return new ResponseModel { Success = false, Message = "Please Email and Password" };
            }
            var user= _context.Users.FirstOrDefault(user => user.Email == userEmail);
            if(user == null)
            {
                return new ResponseModel { Success = false, Message = "please enter correct credential" };
            }    
            var userRole = _context.UserRoles
                .Include(ur => ur.Role)
                .Where(u => u.UserId == user.Id)
                .Select(u => new { u.RoleId, u.Role.Name })
                .FirstOrDefault();
            var roleId = Convert.ToInt32(userRole?.RoleId ?? 0);
            var roleName = userRole?.Name ?? "User";

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var token =  GenerateAccessToken(user.Id, roleId, roleName);
                return new ResponseModel { Success = true, Message = "Login Successfully", Data = token };
            } else
            {
                return new ResponseModel { Success = false, Message = "Credentials aren't correct" };
            }
                
        }
        public string GenerateAccessToken(int id,int roleId, string role)
        {
            var signingKey = _config.GetConnectionString("SigningKey");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!));
            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,id.ToString() ),
                new Claim(ClaimTypes.Role, role),
                new Claim("role_id", roleId.ToString())

            };

            var token = new JwtSecurityToken
            (
              issuer: "ProcurmentProject",
              audience: "ProcurmentProject", 
              expires: DateTime.Now.AddMinutes(15),
              claims: claim,
              signingCredentials: cred
             );
           

            var tokenhandler = new JwtSecurityTokenHandler();
            var accessToken = tokenhandler.WriteToken(token);

       
            return accessToken;
               
        }
    }
}
