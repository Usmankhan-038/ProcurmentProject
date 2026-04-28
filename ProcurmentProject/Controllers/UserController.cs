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

            if(!userSignUp.Success)
            {
                return BadRequest(userSignUp.Message);
            }
            return Ok(userSignUp);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] string userEmail,[FromForm] string password)
        {
            var isLogin = await _user.Login(userEmail, password);
            if (!isLogin.Success)
            {
                return BadRequest(isLogin.Message);
            }
            return Ok(isLogin);
        }
    }
}
