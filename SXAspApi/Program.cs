using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SXAspApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("PhoneConnectionString");
var identityCon = builder.Configuration.GetConnectionString("IdentityConnectionString");

builder.Services.AddControllers();

builder.Services.AddDbContext<PhoneBookContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(identityCon));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
