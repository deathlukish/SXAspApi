using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SXAspApi.Controllers;
using SXAspApi.Models;
using SXAspApi.Services;
using System.Security.Claims;
using System.Text;

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
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "AuthCooke";
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
    opts.Events = new JwtBearerEvents
    {
        OnTokenValidated = async ctx =>
        {
            var usrmgr = ctx.HttpContext.RequestServices
                .GetRequiredService<UserManager<IdentityUser>>();
            var signinmgr = ctx.HttpContext.RequestServices
                .GetRequiredService<SignInManager<IdentityUser>>();
            string username =
                ctx.Principal.FindFirst(ClaimTypes.Name)?.Value;
            IdentityUser idUser = await usrmgr.FindByNameAsync(username);
            ctx.Principal =
                await signinmgr.CreateUserPrincipalAsync(idUser);
        }
    };
});
builder.Services.AddScoped<IPhoneBookService, Phones>();
var app = builder.Build();
SeedData.CreateMigrationBase(app);
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
