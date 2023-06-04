using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SXAspApi.Models
{
    public static class SeedData
    {
        private const string User = "admin";
        private const string Pass = "12345678";
        public static async void CreateMigrationBase(IApplicationBuilder app)
        {
            var context = app.ApplicationServices
                    .CreateScope().ServiceProvider
                    .GetRequiredService<PhoneBookContext>();
            var con = app.ApplicationServices
                    .CreateScope().ServiceProvider
                    .GetRequiredService<IdentityContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (con.Database.GetPendingMigrations().Any())
            {
                con.Database.Migrate();
            }
            UserManager<IdentityUser> manager = app.ApplicationServices
                 .CreateScope().ServiceProvider
                 .GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> _roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            IdentityUser user = await manager.FindByNameAsync(User);
            await _roleManager.CreateAsync(new IdentityRole("admin"));
            await _roleManager.CreateAsync(new IdentityRole("guest"));
            await _roleManager.CreateAsync(new IdentityRole("user"));

            if (user == null)
            {
                user = new IdentityUser(User);
                await manager.CreateAsync(user, Pass);
                await manager.AddToRoleAsync(user, "admin");
            }
            var books = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<PhoneBookContext>();
            if (!books.Notes.Any())
            {
                books.Notes.Add(new SharedLibPhoneBook.PhoneBookDetail()
                {
                    FirsName = "Иван",
                    MiddleName = "Иванович",
                    LastName = "Иванов"                 
                }
                );
                books.Notes.Add(new SharedLibPhoneBook.PhoneBookDetail()
                {
                    FirsName = "Петр",
                    MiddleName = "Петрович",
                    LastName = "Петвов",
                    Phone ="+79999999999"
                    
                }
                );
                books.Notes.Add(new SharedLibPhoneBook.PhoneBookDetail()
                {
                    FirsName = "Сидр",
                    MiddleName = "Сидорович",
                    LastName = "Сидоров"
                    
                }
                );
                books.SaveChanges();
            }
        }
    }
}
