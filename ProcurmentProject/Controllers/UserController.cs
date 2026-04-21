using Microsoft.AspNetCore.Mvc;
using ProcurmentProject.Dto;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Models;


namespace ProcurmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUser _user;
        
        public UserController(IUser user) 
        {
            _user = user;
           
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<string>> SignUp([FromForm] SignUpDto userDetail)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userSignUp = await _user.CreateUser(userDetail);

            if(!userSignUp.success)
            {
                return BadRequest(userSignUp.message);
            }
            return Ok(userSignUp.message);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] string userEmail,[FromForm] string password)
        {
            var isLogin = await _user.Login(userEmail, password);
            if (!isLogin.success)
            {
                return BadRequest(isLogin.message);
            }
            Response.Cookies.Append("X-AccessToken", isLogin.token!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(15)
            });
            return Ok(new { message = isLogin.message, accesstoken = isLogin.token });
        }
    }
}
