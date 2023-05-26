using Microsoft.EntityFrameworkCore;
using SXAspApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("PhoneConnectionString");

builder.Services.AddControllers();

builder.Services.AddDbContext<PhoneBookContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

SeedData.CreateMigrationBase(app);
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
