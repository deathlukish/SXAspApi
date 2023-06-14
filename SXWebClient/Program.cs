using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SXWebClient.Services;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Authentication;

namespace SXWebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            clientHandler.SslProtocols = SslProtocols.None;
            clientHandler.UseCookies = true;
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<BackendApiAuthenticationHttpClientHandler>();
            // Add services to the container.
            builder.Services.AddMvc();
            builder.Services.AddHttpClient<IHomeService, HomeService>()
                            .ConfigureHttpClient((b) => b.BaseAddress = new Uri($"https://127.0.0.1:7227")
                            ).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                            {
                                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                                SslProtocols = SslProtocols.None,
                                UseCookies = true
                            })
                            .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();              
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