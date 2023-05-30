using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedLibPhoneBook;
using System.Net.Http;

namespace SXWebClient.Pages
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        [BindProperty]
        public PhoneBook PhoneBook { get; set; } = new();
        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBook>($"webapi/PhoneBooks/{id}");
            return Page();
        }
    }
}
