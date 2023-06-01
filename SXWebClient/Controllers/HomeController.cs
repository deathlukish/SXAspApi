using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using System.Net.Http;

namespace SXWebClient.Controllers
{
    public class HomeController : Controller
    {
        public IEnumerable<PhoneBook> Notes { get; set; }
        private readonly HttpClient _httpClient;
        public PhoneBook PhoneBook { get; set; } = new();
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
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBook>($"webapi/PhoneBooks/{id}");
            return View(PhoneBook);
        }
    }
}
