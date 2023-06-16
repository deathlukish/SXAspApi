using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Authentication;

namespace SXWebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<DependTokenFromCookies>();
            // Add services to the container.
            builder.Services.AddMvc();  
            builder.Services.AddHttpClient("HomeClient", client =>
                                                   {
                                                   client.BaseAddress = new Uri($"https://127.0.0.1:7227");
                                                   }).ConfigurePrimaryHttpMessageHandler(() =>
                                                   new HttpClientHandler
                                                   {
                                                     ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                                                     SslProtocols = SslProtocols.None,
                                                   }).AddHttpMessageHandler<DependTokenFromCookies>();
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

                //opts.TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateIssuer = true,
                //    ValidIssuer = AuthOptions.ISSUER,
                //    ValidateAudience = true,
                //    ValidAudience = AuthOptions.AUDIENCE,
                //    ValidateLifetime = true,
                //    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                //    ValidateIssuerSigningKey = true,
                //};
                //opts.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = async ctx =>
                //    {
                //        var usrmgr = ctx.HttpContext.RequestServices
                //            .GetRequiredService<UserManager<IdentityUser>>();
                //        var signinmgr = ctx.HttpContext.RequestServices
                //            .GetRequiredService<SignInManager<IdentityUser>>();
                //        string username =
                //            ctx.Principal.FindFirst(ClaimTypes.Name)?.Value;
                //        IdentityUser idUser = await usrmgr.FindByNameAsync(username);
                //        ctx.Principal =
                //            await signinmgr.CreateUserPrincipalAsync(idUser);
                //    }
                //};
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.MapRazorPages();

            app.Run();
        }
    }
}