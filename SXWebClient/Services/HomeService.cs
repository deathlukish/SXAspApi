using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;

namespace SXWebClient.Services
{
    public class HomeService : IHomeService
    {
        private readonly HttpClient _httpClient;
        public HomeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<PhoneBook>> GetNotesFromApi() => await _httpClient.GetFromJsonAsync<List<PhoneBook>>("webapi/PhoneBooks");
        public async Task DeleteNoteFromApi(int id)
        {
            await _httpClient.DeleteAsync($"webapi/PhoneBooks/{id}");

        }
    }
}
