using Bloq.Shared.Exceptions;
using Bloq.Shared.Models;
using Bloq.Shared.Models.Api;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bloq.Client.Services
{
    public interface IAuthorizeClient
    {
        Task<UserModel> GetCurrentUserAsync();
        Task RegisterAsync(RegisterModel model);
        Task LoginAsync(LoginModel model);
        Task LogoutAsync();
    }

    public class AuthorizeClient : IAuthorizeClient
    {
        private readonly HttpClient _client;

        public AuthorizeClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserModel> GetCurrentUserAsync()
        {
            return await _client.GetFromJsonAsync<UserModel>("Authorize/CurrentUser");
        }

        public async Task RegisterAsync(RegisterModel model)
        {
            var result = await _client.PostAsJsonAsync("Authorize/Register", model);
            result.EnsureSuccessStatusCode();

            var registerResponse = await result.Content.ReadFromJsonAsync<RegisterResponse>();
            if (!registerResponse.Success) throw new UserRegistrationException(registerResponse.Errors);
        }

        public async Task LoginAsync(LoginModel model)
        {
            var result = await _client.PostAsJsonAsync("Authorize/Login", model);
            result.EnsureSuccessStatusCode();
        }

        public async Task LogoutAsync()
        {
            var result = await _client.PostAsync("Authorize/Logout", null);
            result.EnsureSuccessStatusCode();
        }
    }
}
