using SharedLibPhoneBook;

namespace SXWebClient.Services
{
    public interface IHomeService
    {
        Task DeleteNoteFromApi(int id);
        Task<IEnumerable<PhoneBook>> GetNotesFromApi();
        Task TestAuth();
        Task<string> GetToken(User user);
    }
}