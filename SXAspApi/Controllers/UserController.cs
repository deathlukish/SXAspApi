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
    [Authorize(Roles = "Admin")]
    [Route("webapi/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        [AllowAnonymous]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] User user)
        {
            IdentityUser use = new();
            if (user.Name != null && user.Password != null)
            {
                use = await _userManager.FindByNameAsync(user.Name);
                if (!await _userManager.CheckPasswordAsync(use, user.Password))
                {
                    return BadRequest("Логин/пароль не распознаны");
                }
            }
            var roles = await _userManager.GetRolesAsync(use);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name), new Claim(ClaimTypes.Role, roles.FirstOrDefault()) };
            var token = new JwtSecurityToken(
                    issuer: "Server",
                    audience: "Client",
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superpupersecurity")), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(encodedJwt);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserApi user)
        {
            if (user != null) 
            {
                var useIdent = new IdentityUser { UserName = user.Name, Email = user.Email };
                await _userManager.CreateAsync(useIdent, user.Password);
                await _userManager.AddToRoleAsync(useIdent, user.Role.RoleName);
            }
            return Ok();
        }

        [HttpDelete("DellUser/{user}")]
        public async Task<IActionResult> DellUser(string user)
        {
            var userIdent = await _userManager.FindByNameAsync(user);
            await _userManager.DeleteAsync(userIdent);
            return Ok();
        }
        [HttpGet("GetAllUSer")]
        public async Task<IActionResult> GetAllUser()
        {
            List<UserApi> Users = new();
            var allUsers = _userManager.Users.ToList();
            foreach (var item in allUsers)
            {
               var role = await _userManager.GetRolesAsync(item);
               Users.Add(new UserApi { Name = item.UserName, Email = item.Email, Role = new RoleApi { RoleName = role.FirstOrDefault() } });
            }
            return Ok(Users);
        }
    }
}
