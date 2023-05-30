using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharedLibPhoneBook;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SXWebClient.Pages
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public CreateModel(HttpClient httpClient)
        { 
        _httpClient = httpClient;
        }
        [BindProperty]
        public PhoneBook PhoneBook { get; set; } = new();
        public void OnGet()
        {
        }
        public async Task OnPost()
        {
            var myContent = JsonSerializer.Serialize(PhoneBook);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await _httpClient.PostAsync("webapi/PhoneBooks", byteContent);
        }
    }
}
