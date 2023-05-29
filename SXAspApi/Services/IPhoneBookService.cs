using SharedLibPhoneBook;
using SXAspApi.Controllers;
using SXAspApi.Models;

namespace SXAspApi.Services
{
    public interface IPhoneBookService
    {
        Task<IEnumerable<PhoneBook>> GetNotes();
        Task AddNote(PhoneBook note);
        Task DeleteNote(int id);
        Task EditNote(PhoneBook note);

    }
}
