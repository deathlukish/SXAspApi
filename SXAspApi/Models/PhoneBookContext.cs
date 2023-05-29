using Microsoft.EntityFrameworkCore;
using SharedLibPhoneBook;

namespace SXAspApi.Models
{
    public class PhoneBookContext : DbContext
    {
        public DbSet<PhoneBook> Notes { get; set; }
        public PhoneBookContext(DbContextOptions<PhoneBookContext> options)
          : base(options)
        {
        }
    }
}
