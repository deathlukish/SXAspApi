using Microsoft.EntityFrameworkCore;
using SharedLibPhoneBook;

namespace SXAspApi.Models
{
    public class PhoneBookContext : DbContext
    {
        public DbSet<PhoneBookDetail> Notes { get; set; }
        public PhoneBookContext(DbContextOptions<PhoneBookContext> options)
          : base(options)
        {
        }
       
    }
}
