using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using SXWebClient.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SXWebClient.Controllers
{
    public class HomeController : Controller
    {
        public IEnumerable<PhoneBook>? Notes { get; set; }
        private readonly HttpClient _httpClient;
        private readonly IHomeService _homeService;
        public PhoneBookDetail PhoneBook { get; set; } = new();
        public HomeController(HttpClient httpClient, IHomeService homeService)
        {
            _httpClient = httpClient;
            _homeService = homeService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                Notes = await _homeService.GetNotesFromApi();
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
        public async Task<IActionResult> Edit(int? id)
        {
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}");
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
        public async Task<IActionResult> Remove(int id) => View(await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}"));
        [HttpPost, ActionName("Remove")]
        public async Task<IActionResult> Delete(int id)
        {
            await _homeService.DeleteNoteFromApi(id);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}");
            return View(PhoneBook);
        }
        public async Task<IActionResult> Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(int id, [Bind("Id,FirstName,MiddleName,LastName,Phone,Description, Adres")] PhoneBookDetail note)
        {
            await _httpClient.PostAsJsonAsync("webapi/PhoneBooks", note);
            return Redirect("index");
        }
        [HttpGet]
        public async Task<IActionResult> Users()
        { 
            var a = await _httpClient.GetAsync("webapi/PhoneBooks/Test");
            if (a.StatusCode == HttpStatusCode.Unauthorized)
            {
                return View("Auth");
            }
            return View("UserManager");
        }
        [HttpPost]
        public async Task<IActionResult> Users(User users)
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
            var a =  await _httpClient.PostAsJsonAsync("webapi/UserAuth", users);
            if (a.IsSuccessStatusCode)
            {
                var b = await a.Content.ReadAsStringAsync();
                Response.Cookies.Append("jwt", b);
            }
           return View("Auth",users);
        }

    }
}
