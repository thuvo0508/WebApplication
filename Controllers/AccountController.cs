using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication.Helpers;
using WebApplication.Models;
namespace WebApplication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        public AccountController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }
        List<Users> logins = new List<Users>() {
            new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "user1@gmail.com",
                        UserName = "user1",
                        Password = "123",
                        Name = "User 1"
                },
                new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "user2@gmail.com",
                        UserName = "user2",
                        Password = "123",
                        Name = "User 2"
                }
        };
        [HttpPost]
        public IActionResult GetToken(UserLogins userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var Valid = logins.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                if (Valid)
                {
                    var user = logins.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                    Token = Helpers.JwtHelpers.GenTokenkey(new UserTokens()
                    {
                        EmailId = user.EmailId,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = user.Id,
                    }, jwtSettings);
                }
                else
                {
                    return BadRequest($"Wrong password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Get List of UserAccounts
        /// </summary>
        /// <returns>List Of UserAccounts</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            return Ok(logins);
        }

        [HttpPost]
        public IActionResult Register([FromBody] UserRegisters registerUser)
        {
            var existUser = logins.Any(x => x.Name == registerUser.Name);
            if (existUser) return BadRequest($"User already exists!");

            logins.Add(new Users()
            {
                Name = registerUser.Name,  
                EmailId = registerUser.EmailId,
                Password = registerUser.Password,
                UserName = registerUser.UserName,
            });
            return Ok("User created successfully!");
        }
    }
}