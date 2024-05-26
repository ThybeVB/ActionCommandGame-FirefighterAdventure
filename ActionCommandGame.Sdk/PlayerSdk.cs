using System.Net.Http.Json;
using ActionCommandGame.Model;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Abstractions;

namespace ActionCommandGame.Sdk
{
    public class PlayerSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public PlayerSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<IList<Player>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = "/api/Player";
            var bearer = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearer);
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var players = await response.Content.ReadFromJsonAsync<IList<Player>>();
            if (players is null)
            {
                return new List<Player>();
            }
            return players;
        }

        public async Task<Player?> Get(string id)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"/api/Player/{id}";
            var bearer = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearer);
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var player = await response.Content.ReadFromJsonAsync<Player>();
            return player;
        }

        public async Task<Player?> Create(Player player)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"api/Player";
            var bearer = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearer);
            var response = await httpClient.PostAsJsonAsync(route, player);

            response.EnsureSuccessStatusCode();

            var newPlayer = await response.Content.ReadFromJsonAsync<Player>();
            return newPlayer;
        }

        public async Task<Player?> Update(string id, Player player)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"api/Player/{id}";
            var bearer = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearer);
            var response = await httpClient.PutAsJsonAsync(route, player);

            response.EnsureSuccessStatusCode();

            var updatedPlayer = await response.Content.ReadFromJsonAsync<Player>();
            return updatedPlayer;
        }

        public async Task Delete(string id)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"/api/Player/{id}";
            var bearer = _tokenStore.GetToken();
            httpClient.AddAuthorization(bearer);
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
