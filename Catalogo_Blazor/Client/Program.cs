using Catalogo_Blazor.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Catalogo_Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<TokenAuthenticationProvider>();
            builder.Services.AddScoped<IAuthorizeService, TokenAuthenticationProvider>(
                provider => provider.GetRequiredService<TokenAuthenticationProvider>());
            builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationProvider>(
                provider => provider.GetRequiredService<TokenAuthenticationProvider>());

            await builder.Build().RunAsync();
        }
    }
}
