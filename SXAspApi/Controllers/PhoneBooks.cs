using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SXAspApi.Models;
using SXAspApi.Services;

namespace SXAspApi.Controllers
{
    [Route("webapi/[controller]")]
    [ApiController]
    public class PhoneBooks : ControllerBase
    {
        private IPhoneBookService _phoneBookService;
        public PhoneBooks(IPhoneBookService phoneBookService)
        { 
            _phoneBookService = phoneBookService;
        }
        //[HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<PhoneBooks>> GetNotes() => await _phoneBookService.GetNotes();
    }
}
