using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using System.Net.Http;

namespace SXWebClient.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory = null!;
        public IEnumerable<UserApi> UserApis { get; set; }
        public UserManagerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("HomeClient" ?? "");
        }
        public async Task<IActionResult> Index()
        {
            UserApis = await _httpClient.GetFromJsonAsync<List<UserApi>>("webapi/user/getalluser");
            return View(UserApis);
        }
    }
}
