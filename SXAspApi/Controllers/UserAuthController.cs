using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedLibPhoneBook;
using SXAspApi.Migrations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Text;

namespace SXAspApi.Controllers
{
    [Route("webapi/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _sign;
        public UserAuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> sign)
        {
            _userManager = userManager;
            _sign = sign;
        }
        //[HttpPost]
        //public async Task<IActionResult> Login([FromBody] User user)
        //{




        //    if (user.UserId != null && user.Password != null)
        //    {
        //        var use = await _userManager.FindByNameAsync(user.UserId);
        //        var valid = await _sign.UserManager.CheckPasswordAsync(use, user.Password);

        //    }
        //    return Ok();
        //}
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            //var identity = GetIdentity(user.UserId, user.Password);
            //if (identity == null)
            //{
            //    return BadRequest(new { errorText = "Invalid username or password." });
            //}

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserId) };
            var token = new JwtSecurityToken(
                    issuer: "server",
                    audience: "client",
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superpupersecurity")), SecurityAlgorithms.HmacSha256));
            return Ok(token);
        }

        //private ClaimsIdentity GetIdentity(string username, string password)
        //{
        //    Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
        //    if (person != null)
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
        //            new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
        //        };
        //        ClaimsIdentity claimsIdentity =
        //        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
        //            ClaimsIdentity.DefaultRoleClaimType);
        //        return claimsIdentity;
        //    }

        //    // если пользователя не найдено
        //    return null;
        //}
    }
}
