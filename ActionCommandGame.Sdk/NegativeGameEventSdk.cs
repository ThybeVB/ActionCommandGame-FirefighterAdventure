using ActionCommandGame.Model;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Abstractions;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class NegativeGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public NegativeGameEventSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<IList<NegativeGameEvent>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = "/api/NegativeGameEvent";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var negativeGameEvents = await response.Content.ReadFromJsonAsync<IList<NegativeGameEvent>>();
            if (negativeGameEvents is null)
            {
                return new List<NegativeGameEvent>();
            }
            return negativeGameEvents;
        }

        public async Task<NegativeGameEvent?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"/api/NegativeGameEvent/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var negativeGameEvent = await response.Content.ReadFromJsonAsync<NegativeGameEvent>();
            return negativeGameEvent;
        }

        public async Task<NegativeGameEvent?> Create(NegativeGameEvent request)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"api/NegativeGameEvent";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var negativeGameEvent = await response.Content.ReadFromJsonAsync<NegativeGameEvent>();
            return negativeGameEvent;
        }

        public async Task<NegativeGameEvent?> Update(int id, NegativeGameEvent request)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"api/NegativeGameEvent/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var updatedEvent = await response.Content.ReadFromJsonAsync<NegativeGameEvent>();
            return updatedEvent;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"/api/NegativeGameEvent/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
