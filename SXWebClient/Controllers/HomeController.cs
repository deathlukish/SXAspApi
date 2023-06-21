using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SXWebClient.Controllers
{
    public class HomeController : Controller
    {
        public IEnumerable<PhoneBook>? Notes { get; set; }
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory = null!;
        public PhoneBookDetail? PhoneBook { get; set; } = new();
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("HomeClient" ?? "");
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                Notes = await _httpClient.GetFromJsonAsync<List<PhoneBook>>("webapi/PhoneBooks");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
            finally
            {
                _httpClient?.Dispose();
            }
            if (Notes != null)
            {
                return View(Notes);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}"); ;
            return View(PhoneBook);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([Bind("Id,FirstName,MiddleName,LastName,Phone,Description,Adres")] PhoneBookDetail note)
        {
            var serialize = JsonSerializer.Serialize(note);
            var requestContent = new StringContent(serialize, Encoding.UTF8, "application/json-patch+json");
            await _httpClient.PatchAsync($"webapi/PhoneBooks/", requestContent);
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            var a = await _httpClient.GetAsync($"webapi/PhoneBooks/{id}");
            if (a.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login");
            }
            return View(await a.Content.ReadFromJsonAsync<PhoneBookDetail>());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Remove")]
        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"webapi/PhoneBooks/{id}");
            return RedirectToAction("index");
        }
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Details(int id)
        {
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}");
            return View(PhoneBook);
        }
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Create() => View();
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Phone,Description, Adres")] PhoneBookDetail note)
        {
            await _httpClient.PostAsJsonAsync("webapi/PhoneBooks", note);
            return Redirect("index");
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                var a = await _httpClient.PostAsJsonAsync("webapi/User/GetToken", user);
                if (a.IsSuccessStatusCode)
                {
                    var b = await a.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(b))
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadJwtToken(b);
                        var c = jsonToken.Claims;
                        var claimsIdentity = new ClaimsIdentity(c, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                       new ClaimsPrincipal(claimsIdentity));
                        Response.Cookies.Append("jwt", b);
                        return Redirect(user.ReturnUrl);
                    }
                }              
            }
            ViewBag.ErrorMessage = "Логин/пароль не распознаны";
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login([FromQuery] string ReturnUrl) => View("Login", new User { ReturnUrl = ReturnUrl });
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            Response.Cookies.Delete("jwt");
            return Redirect("index");
        }
    }
}
