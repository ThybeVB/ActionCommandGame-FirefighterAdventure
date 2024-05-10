using ActionCommandGame.Services.Model.Requests.Identity;
using System.Net.Http.Json;
using ActionCommandGame.Services.Model.Results.Identity;

namespace ActionCommandGame.Sdk
{
    public class IdentitySdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentitySdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<JwtAuthenticationResult?> SignIn(UserSignInRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = "/api/Identity/sign-in";
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();
        }

        public async Task<JwtAuthenticationResult?> Register(UserRegisterRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = "/api/Identity/register";
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();
        }
    }
}
