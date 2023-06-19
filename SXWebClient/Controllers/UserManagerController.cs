using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SXWebClient.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
