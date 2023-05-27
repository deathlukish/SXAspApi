using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SXAspApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("PhoneConnectionString");
var identityCon = builder.Configuration.GetConnectionString("IdentityConnectionString");

builder.Services.AddControllers();

builder.Services.AddDbContext<PhoneBookContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(identityCon));
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<IdentityContext>();
var app = builder.Build();
SeedData.CreateMigrationBase(app);
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
