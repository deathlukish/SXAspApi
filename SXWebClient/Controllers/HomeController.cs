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
        public PhoneBookDetail PhoneBook { get; set; } = new();
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
        public async Task<IActionResult> Edit(int id)
        {          
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}"); ;           
            return View(PhoneBook);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Phone,Description,Adres")] PhoneBookDetail note)
        {
            var serialize = JsonSerializer.Serialize(note);
            var requestContent = new StringContent(serialize, Encoding.UTF8, "application/json-patch+json");           
            await _httpClient.PatchAsync($"webapi/PhoneBooks/", requestContent);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Remove(int id)
        {            
            var a = await _httpClient.GetAsync($"webapi/PhoneBooks/{id}");
            if (a.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login");
            }           
            return View(await a.Content.ReadFromJsonAsync<PhoneBookDetail>());
        }
        [HttpPost, ActionName("Remove")]
        public async Task<IActionResult> Delete(int id)
        {           
            var a = await _httpClient.DeleteAsync($"webapi/PhoneBooks/{id}");            
            return RedirectToAction("index");
        }
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {        
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}");           
            return View(PhoneBook);
        }
        public async Task<IActionResult> Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Phone,Description, Adres")] PhoneBookDetail note)
        {           
            await _httpClient.PostAsJsonAsync("webapi/PhoneBooks", note);
            return Redirect("index");
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                string message = "fds";
                if (message.Equals("1"))
                {

                }
                else
                {
                    ViewBag.ErrorMessage = message;
                }
            }           
            var a = await _httpClient.PostAsJsonAsync("webapi/UserAuth/login", user);
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
                }
            }
            return Redirect("Index");
        }
        public async Task<IActionResult> Login([FromQuery]string red)
        {
            var a = red;
            return View("Login");
        }
    }
}
