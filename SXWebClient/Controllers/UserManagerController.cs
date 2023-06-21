using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace SXWebClient.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory = null!;
        public IEnumerable<UserApi> UserApis { get; set; }
        public UserManagerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("HomeClient" ?? "");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                await _httpClient.DeleteAsync($"webapi/user/delluser/{name}");
            }
            return Redirect("index");
        }
        public async Task<IActionResult> Index()
        {
            UserApis = await _httpClient.GetFromJsonAsync<List<UserApi>>("webapi/user/getalluser");
            return View(UserApis);
        }
        public IActionResult NewUser()
        { 
           return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewUser(NewUser user)
        {
            if (ModelState.IsValid)
            {
                var a = await _httpClient.PostAsJsonAsync("webapi/User/AddUser", user);
                if (a.IsSuccessStatusCode)
                {
                    return Redirect("index");
                }
            }
            ViewBag.ErrorMessage = "Логин/пароль не распознаны";
            return View();
        }

    }
}
