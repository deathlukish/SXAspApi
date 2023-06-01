using System.Net.Http.Headers;
using System.Security.Authentication;

namespace SXWebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // builder.Services.AddRazorPages();
            builder.Services.AddMvc();
            builder.Services.AddScoped(sp =>
            {            
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
                clientHandler.SslProtocols = SslProtocols.None;                
                var client = new HttpClient(clientHandler) { BaseAddress = new Uri($"https://127.0.0.1:7227") };
                return client;
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