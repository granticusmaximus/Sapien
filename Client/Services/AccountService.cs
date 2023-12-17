using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Register(RegisterModel model)
        {
            return await _httpClient.PostAsJsonAsync("api/account/register", model);
        }

        public async Task<HttpResponseMessage> Login(LoginModel model)
        {
            return await _httpClient.PostAsJsonAsync("api/account/login", model);
        }

        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordModel model)
        {
            return await _httpClient.PostAsJsonAsync("api/account/changePassword", model);
        }

        // Additional methods as needed...
    }
}