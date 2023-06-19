using Microsoft.AspNetCore.Mvc;

namespace SXWebClient.Controllers
{
    public class UserManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
