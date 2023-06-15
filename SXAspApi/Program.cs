using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SXAspApi.Models;
using SXAspApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme =
        JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "Server",
        ValidateAudience = true,
        ValidAudience = "Client",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superpupersecurity")),
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddScoped<IPhoneBookService, Phones>();
var app = builder.Build();
SeedData.CreateMigrationBase(app);
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
