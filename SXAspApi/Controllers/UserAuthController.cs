using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using SXAspApi.Migrations;

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
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (user.UserId != null && user.Password != null)
            {
                var use = await _userManager.FindByNameAsync(user.UserId);
                var valid = await _sign.UserManager.CheckPasswordAsync(use, user.Password);

            }
            return Ok();
        }
    }
}
