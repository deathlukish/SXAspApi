using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using System.Net.Http;
using System.Text.Json;

namespace SXWebClient.Controllers
{
    public class HomeController : Controller
    {
        public IEnumerable<PhoneBook> Notes { get; set; }
        private readonly HttpClient _httpClient;
        public PhoneBookDetail PhoneBook { get; set; } = new();
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                Notes = await _httpClient?.GetFromJsonAsync<List<PhoneBook>>("webapi/PhoneBooks");
            }
            catch (Exception ex)
            {
          
                
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Phone,Description, Adres")] PhoneBookDetail note)
        {
            var a = note;
            return View();
        }
        public async Task<IActionResult> Remove(int id)
        { 
        return View(await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}"));
        }
        public async Task<IActionResult> Details(int? id)
        {
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}");
            return View(PhoneBook);
        }
        public async Task<IActionResult> Create()
        {           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(int id, [Bind("Id,FirstName,MiddleName,LastName,Phone,Description, Adres")] PhoneBookDetail note)
        {

          await _httpClient.PostAsJsonAsync<PhoneBookDetail>("webapi/PhoneBooks", note);  
          return Redirect("index");
        }
    }
}
