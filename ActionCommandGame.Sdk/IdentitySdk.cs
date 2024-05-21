using ActionCommandGame.Services.Model.Requests.Identity;
using System.Net.Http.Json;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Results.Identity;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Sdk
{
    public class IdentitySdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentitySdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<JwtAuthenticationResult?> SignIn(UserSignInRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "https://localhost:7065/api/Identity/signin"; //todo wrm??
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();

            if (result is null)
            {
                return new JwtAuthenticationResult()
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "ApiError", Message = "An API error occurred." }
                    }
                };
            }

            return result;
        }

        public async Task<JwtAuthenticationResult?> Register(UserRegisterRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = "https://localhost:7065/api/Identity/register"; //todo wrm????
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JwtAuthenticationResult>();
            if (result is null)
            {
                return new JwtAuthenticationResult()
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "ApiError", Message = "An API error occurred." }
                    }
                };
            }

            return result;
        }
    }
}
