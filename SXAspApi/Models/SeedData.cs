using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SXAspApi.Models
{
    public static class SeedData
    {
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
                context.Database.Migrate();
            }
        }
    }
}
