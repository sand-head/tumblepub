using Bloq.Client.Services;
using Bloq.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bloq.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddHttpClient<IAuthorizeClient, AuthorizeClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddSingleton<RenderingContext>();

            ConfigureSharedServices(builder.Services);
            ConfigureAuthorization(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureSharedServices(IServiceCollection services)
        {
            // I don't really understand why Microsoft said I should make this
            // if they weren't going to populate it with anything
            // I guess I'll keep it, cause I'll probably need it at some point
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddOptions();
            services.AddAuthorizationCore();

            services.TryAddScoped<IdentityAuthenticationStateProvider>();
            services.TryAddScoped<AuthenticationStateProvider>(services => services.GetRequiredService<IdentityAuthenticationStateProvider>());

            services.TryAddTransient<BaseAddressAuthorizationMessageHandler>();
            services.TryAddTransient<AuthorizationMessageHandler>();
        }
    }
}
