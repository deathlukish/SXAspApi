using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedLibPhoneBook;
using System.Net.Http;

namespace SXWebClient.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public PhoneBook DatailNote { get; set; }
        public string Error { get; set; }
        public DetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                DatailNote = await _httpClient?.GetFromJsonAsync<PhoneBook>("webapi/PhoneBooks");
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                DatailNote = new PhoneBook();
            }
            finally
            {
                _httpClient?.Dispose();
            }
            //if (Notes == null)
            //{ 
            //return NotFound();
            //}
            return Page();
        }
    }
}

