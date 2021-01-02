using Bloq.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bloq.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddHttpClient("Bloq.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Bloq.ServerAPI"));

            builder.Services.AddApiAuthorization();

            builder.Services.AddSingleton<RenderingContext>();
    
            ConfigureSharedServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureSharedServices(IServiceCollection services)
        {
            // I don't really understand why Microsoft said I should make this
            // if they weren't going to populate it with anything
            // I guess I'll keep it, cause I'll probably need it at some point
        }
    }
}
