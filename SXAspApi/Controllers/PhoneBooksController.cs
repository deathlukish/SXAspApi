﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
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
        public async Task<IActionResult> GetNotes()
        {
            try
            {
                return Ok(await _phoneBookService.GetNotes());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }            

        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                await _phoneBookService.DeleteNote(id);
            }
            catch (Exception)
            {

              return StatusCode(StatusCodes.Status500InternalServerError);

            }
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> AddNote([FromBody] PhoneBookDetail note)
        {
            await _phoneBookService.AddNote(note);
            return Ok();
        }
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditNote([FromBody] PhoneBookDetail note)
        {
            await _phoneBookService.EditNote(note);
            return Ok();
        }
        [Authorize(Roles = "User, Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            try
            {
                var note = await _phoneBookService.GetNoteById(id);
                if (note == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(await _phoneBookService.GetNoteById(id));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }        
    }
}
