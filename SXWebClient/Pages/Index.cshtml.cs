using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using SharedLibPhoneBook;
namespace SXWebClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public string Error { get; set; }
        public IEnumerable<PhoneBook> Notes { get; set; }
        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Notes = await _httpClient?.GetFromJsonAsync<List<PhoneBook>>("webapi/PhoneBooks");
            }
            catch (Exception ex) 
            {
            Error = ex.Message;
            Notes = new List<PhoneBook>();
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
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _httpClient.DeleteAsync($"webapi/PhoneBooks/{id}");
            return RedirectToPage();
        }
    }
}