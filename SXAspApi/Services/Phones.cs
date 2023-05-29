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
        public Task AddNote(PhoneBook note)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNote(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditNote(PhoneBook note)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PhoneBook>> GetNotes()
        {
            return _dbContext.Notes;
        }
    }
}
