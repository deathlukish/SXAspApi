using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using System.Net;
using System.Net.Http.Json;
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
        public HomeController(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                using HttpClient client = _httpClientFactory.CreateClient("HomeClient" ?? "");
                Notes = await client.GetFromJsonAsync<List<PhoneBook>>("webapi/PhoneBooks");
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
            using HttpClient client = _httpClientFactory.CreateClient("HomeClient" ?? "");
            PhoneBook = await client.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}"); ;           
            return View(PhoneBook);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Phone,Description,Adres")] PhoneBookDetail note)
        {
            var serialize = JsonSerializer.Serialize(note);
            var requestContent = new StringContent(serialize, Encoding.UTF8, "application/json-patch+json");
            using HttpClient client = _httpClientFactory.CreateClient("HomeClient" ?? "");
            await client.PatchAsync($"webapi/PhoneBooks/", requestContent);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Remove(int id)
        {
            using HttpClient client = _httpClientFactory.CreateClient("HomeClient" ?? "");
            var a = await client.GetAsync($"webapi/PhoneBooks/{id}");
            if (a.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Auth");
            }           
            return View(await a.Content.ReadFromJsonAsync<PhoneBookDetail>());
        }
        [HttpPost, ActionName("Remove")]
        public async Task<IActionResult> Delete(int id)
        {

            HttpResponseMessage httpResponse = new();
            using HttpClient client = _httpClientFactory.CreateClient("HomeClient" ?? "");
            var a = await client.DeleteAsync($"webapi/PhoneBooks/{id}");
            
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Details(int id)
        {
            using HttpClient client = _httpClientFactory.CreateClient("HomeClient" ?? ""); 
            PhoneBook = await client.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}");           
            return View(PhoneBook);
        }
        public async Task<IActionResult> Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Phone,Description, Adres")] PhoneBookDetail note)
        {
            using HttpClient client = _httpClientFactory.CreateClient("HomeClient" ?? "");
            await client.PostAsJsonAsync("webapi/PhoneBooks", note);
            return Redirect("index");
        }
        [HttpPost]
        public async Task<IActionResult> Auth(User user)
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
            using HttpClient client = _httpClientFactory.CreateClient("HomeClient" ?? "");
            var a = await client.PostAsJsonAsync("webapi/UserAuth/login", user);
            //if (a.IsSuccessStatusCode)
            //{
            //    var b = await a.Content.ReadAsStringAsync();
           Response.Cookies.Append("jwt", await a.Content.ReadAsStringAsync());
            //}
           return View("Auth",user);
        }
        public async Task<IActionResult> Auth()
        {
            return View("Auth");
        }
    }
}
