using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SXAspApi.Models
{
    public static class SeedData
    {
        private const string User = "user";
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
            await _roleManager.CreateAsync(new IdentityRole("root"));
            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            await _roleManager.CreateAsync(new IdentityRole("user"));

            if (user == null)
            {
                user = new IdentityUser(User);
                user.Email = "";
                user.PhoneNumber = "";
                await manager.CreateAsync(user, Pass);
                await manager.AddToRoleAsync(user, "root");
            }
        }
    }
}
