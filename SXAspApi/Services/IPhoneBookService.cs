using SharedLibPhoneBook;
using SXAspApi.Controllers;
using SXAspApi.Models;

namespace SXAspApi.Services
{
    public interface IPhoneBookService
    {
        Task<IEnumerable<PhoneBook>> GetNotes();
        Task AddNote(PhoneBookDetail note);
        Task DeleteNote(int id);
        Task EditNote(PhoneBookDetail note);
        Task<PhoneBookDetail> GetNoteById(int id);

    }
}
