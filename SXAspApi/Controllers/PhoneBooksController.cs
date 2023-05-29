﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using SXAspApi.Models;
using SXAspApi.Services;

namespace SXAspApi.Controllers
{
    [Route("webapi/[controller]")]
    [ApiController]
    public class PhoneBooksController : ControllerBase
    {
        private IPhoneBookService _phoneBookService;
        public PhoneBooksController(IPhoneBookService phoneBookService)
        { 
            _phoneBookService = phoneBookService;
        }
        //[HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<PhoneBook>> GetNotes() => await _phoneBookService.GetNotes();
    }
}