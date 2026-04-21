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
        public async Task<(bool success, string message)> CreateUser(SignUpDto signup)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (signup == null)
                {
                    return (false, "The Data is incomplete. Please Enter Complete Data");
                } else if (signup.password != signup.confirmpassword)
                {
                    return (false, "Password and confirm password must be same");
                }

                signup.password = BCrypt.Net.BCrypt.HashPassword(signup.password);

                bool isEmailExist = _context.Users.Any(user => user.Email == signup.email);
                if (isEmailExist)
                {
                    return (false, "This email is already exist,use another email please");
                }
                var user = new User
                {
                    Name = signup.name,
                    Email = signup.email,
                    Phone = signup.phone,
                    Password = signup.password,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                

                _context.Users.Add(user);
                await _context.SaveChangesAsync();


                var company = _context.Companies.FirstOrDefault(user => user.Name == signup.companyName);

                if(company == null)
                {
                    return (false, "Company you enter is not Valid");
                }

                var userCompany = new UserCompany
                {
                    UserId = user.Id,
                    CompanyId = company!.Id,
                };

                _context.UserCompanies.Add(userCompany);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return (true, "User Created Successfully");
            } catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Something went Wrong, {ex.Message}");
            }
        }
        public async Task<(bool success,string? token, string message)> Login(string userEmail, string password)
        {
            if(userEmail == null || password == null)
            {
                return (false,null,"Please Email and Password");
            }
            var user= _context.Users.FirstOrDefault(user => user.Email == userEmail);
            if(user == null)
            {
                return (false, null, "please enter correct credential");
            }    
            var userRole = _context.UserRoles
                .Include(ur => ur.Role)
                .Where(u => u.UserId == user.Id)
                .Select(u => u.Role.Name)
                .FirstOrDefault() ?? "User";

            if (user != null || BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var token =  GenerateAccessToken(user.Id,userRole);
                return (true,token,"Login Successfully");
            } else
            {
                return (false, null, "Credentials aren't correct");
            }
                
        }
        public string GenerateAccessToken(int id,string role)
        {
            var signingKey = _config.GetConnectionString("SigningKey");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!));
            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,id.ToString() ),
                new Claim(ClaimTypes.Role, role)

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
