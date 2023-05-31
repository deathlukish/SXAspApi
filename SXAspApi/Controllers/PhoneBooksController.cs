using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        public async Task<IActionResult> DeleteNote(int id)
        {
            await _phoneBookService.DeleteNote(id);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] PhoneBook note)
        {
            await _phoneBookService.AddNote(note);
            return Ok();
        }
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
