using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using System.Net.Http.Headers;

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
        public async Task TestAuth()
        {
            var a = await _httpClient.GetAsync("webapi/PhoneBooks/Test");
        
        }
        public async Task<string> GetToken(User user)
        {
            var a = await _httpClient.PostAsJsonAsync("webapi/UserAuth", user);
            if (a.IsSuccessStatusCode)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await a.Content.ReadAsStringAsync());
                return await a.Content.ReadAsStringAsync(); 
            }
            return null;
        }
    }
}
