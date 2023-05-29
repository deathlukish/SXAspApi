using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SXWebClient.Pages
{
    public class UsersModel : PageModel
    {
        private readonly ILogger<UsersModel> _logger;

        public UsersModel(ILogger<UsersModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}