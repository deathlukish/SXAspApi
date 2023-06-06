using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using SXAspApi.Controllers;
using SXAspApi.Models;

namespace SXAspApi.Services
{
    public class Phones : IPhoneBookService
    {
        private readonly PhoneBookContext _dbContext;
        public Phones(PhoneBookContext context)
        {
            _dbContext = context;
        }
        public async Task AddNote(PhoneBookDetail note)
        {
           await _dbContext.AddAsync(note);
           await _dbContext.SaveChangesAsync();
        }       
        public async Task DeleteNote(int id)
        {
             _dbContext.Notes.Remove(_dbContext?.Notes?.FirstOrDefault(x => x.Id == id));
            await _dbContext.SaveChangesAsync();           
        }

        public async Task EditNote(PhoneBookDetail note)
        {
            var a = _dbContext.Notes.FirstOrDefault(x => x.Id == note.Id);
            if (a != null) 
            {
                a.LastName = note.LastName;
                a.FirstName = note.FirstName;
                a.MiddleName = note.MiddleName;
                a.Phone = note.Phone;
                a.Description = note.Description;
                a.Adres = note.Adres;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PhoneBookDetail> GetNoteById(int id)
        {
            var a = _dbContext.Notes.FirstOrDefault(x => x.Id == id);
            return a;
        }

        public async Task<IEnumerable<PhoneBook>> GetNotes() => _dbContext.Notes;

    }
}
