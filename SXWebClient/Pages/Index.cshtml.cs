using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using SharedLibPhoneBook;
namespace SXWebClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public IEnumerable<PhoneBook> Notes { get; set; }
        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public void OnGet()
        {                
           Notes =  _httpClient?.GetFromJsonAsync<List<PhoneBook>>("webapi/PhoneBooks").Result;           
        }
    }
}