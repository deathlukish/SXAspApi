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
            builder.Services.AddAuthentication(opts => opts.DefaultScheme =
        CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = "/home/login";
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

            app.UseAuthentication();
            app.UseAuthorization();
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