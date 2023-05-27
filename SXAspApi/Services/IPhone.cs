using SXAspApi.Controllers;

namespace SXAspApi.Services
{
    public class IPhone : IPhoneBookService
    {
        public Task AddNote(PhoneBooks note)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNote(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditNote(PhoneBooks note)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PhoneBooks>> GetNotes()
        {
            throw new NotImplementedException();
        }
    }
}
