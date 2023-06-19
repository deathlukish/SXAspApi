using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedLibPhoneBook;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SXAspApi.Controllers
{
    [Route("webapi/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _sign;
        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> sign)
        {
            _userManager = userManager;
            _sign = sign;
        }
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] User user)
        {
            if (user.UserId != null && user.Password != null)
            {
                var use = await _userManager.FindByNameAsync(user.UserId);
                if (!await _sign.UserManager.CheckPasswordAsync(use, user.Password))
                {
                    return BadRequest("Логин/пароль не распознаны");
                }
            }
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Testing"), new Claim(ClaimTypes.Role, "Admin") };
            var token = new JwtSecurityToken(
                    issuer: "Server",
                    audience: "Client",
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superpupersecurity")), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(encodedJwt);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            return Ok();
        }
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            List<User> Users = new(); 
            foreach (var item in _sign.UserManager.Users)
            {
                //await _userManager.GetRolesAsync(item);
                Users.Add(new User { UserId = item.Id });
                
            }
            return Ok(Users);
        }
    }
}
