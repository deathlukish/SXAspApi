using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedLibPhoneBook;

namespace SXWebClient.Pages
{
    public class RemoveModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public string ErrorMessage { get; set; }
        public PhoneBook Note { get; set; }
        public RemoveModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnGetAsync(int id, bool? saveChangesError)
        {
            var resp = await _httpClient.GetAsync($"webapi/PhoneBooks/{id}");
            if (resp.IsSuccessStatusCode)
            {
                Note = await resp.Content.ReadFromJsonAsync<PhoneBook>();

            }
            else
            { 
                ErrorMessage = resp.ReasonPhrase ?? string.Empty;
                Note = new();
            }

            return Page();
           
        }
    }
}
