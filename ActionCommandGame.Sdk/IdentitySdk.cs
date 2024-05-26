using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests.Identity;
using ActionCommandGame.Services.Model.Results.Identity;
using System.Net.Http.Json;

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
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = "/api/Identity/signin";
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
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = "/api/Identity/register";
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

        public async Task<JwtAuthenticationResult?> Update(UserRegisterRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = "/api/Identity/update";
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
