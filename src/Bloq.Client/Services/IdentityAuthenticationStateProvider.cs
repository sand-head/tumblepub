using Bloq.Shared.Models;
using Bloq.Shared.Models.Api;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bloq.Client.Services
{
    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider, IAuthorizeClient
    {
        private readonly ILogger<IdentityAuthenticationStateProvider> _logger;
        private readonly IAuthorizeClient _client;
        private UserModel _user;

        public IdentityAuthenticationStateProvider(ILogger<IdentityAuthenticationStateProvider> logger, IAuthorizeClient client)
        {
            _logger = logger;
            _client = client;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            try
            {
                var user = await GetCurrentUserAsync();
                if (user.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.Username) }
                        .Concat(user.Claims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server Authentication");
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Failed to get authentication state: {Message}", e.Message);
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task<UserModel> GetCurrentUserAsync()
        {
            if (_user is not null && _user.IsAuthenticated) return _user;
            _user = await _client.GetCurrentUserAsync();
            return _user;
        }

        public async Task RegisterAsync(RegisterModel model)
        {
            await _client.RegisterAsync(model);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LoginAsync(LoginModel model)
        {
            await _client.LoginAsync(model);
            _user = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LogoutAsync()
        {
            await _client.LogoutAsync();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
