using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using Template_UI_WASM.Service;
using Template_UI_WASM.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Template_UI_WASM.Contracts;

namespace Template_UI_WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<ApiAuthenticationStateProvider>());
            builder.Services.AddScoped<JwtSecurityTokenHandler>();
            builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
            builder.Services.AddTransient<IAuthorRepository, AuthorRepository>();
            builder.Services.AddTransient<IBookRepository, BookRepository>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            //builder.Services.AddTransient<IFileUpload, FileUpload>();
            await builder.Build().RunAsync();
        }
    }
}
