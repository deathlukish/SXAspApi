using SXAspApi.Controllers;
using SXAspApi.Models;

namespace SXAspApi.Services
{
    public interface IPhoneBookService
    {
        Task<IEnumerable<PhoneBooks>> GetNotes();
        Task AddNote(PhoneBooks note);
        Task DeleteNote(int id);
        Task EditNote(PhoneBooks note);

    }
}
