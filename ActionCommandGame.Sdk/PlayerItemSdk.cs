using ActionCommandGame.Model;
using System.Net.Http.Json;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Sdk.Extensions;

namespace ActionCommandGame.Sdk
{
    public class PlayerItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public PlayerItemSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<IList<PlayerItem>> Find(string? currentPlayerId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/PlayerItem/find";
            if (!string.IsNullOrWhiteSpace(currentPlayerId))
            {
                route = $"/api/PlayerItem/find/{currentPlayerId}";
            }
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);

            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var players = await response.Content.ReadFromJsonAsync<IList<PlayerItem>>();
            if (players is null)
            {
                return new List<PlayerItem>();
            }
            return players;
        }

        public async Task<PlayerItem?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/PlayerItem/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var player = await response.Content.ReadFromJsonAsync<PlayerItem>();
            return player;
        }

        public async Task<PlayerItem?> Create(PlayerItem playerItem)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/PlayerItem";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.PostAsJsonAsync(route, playerItem);

            response.EnsureSuccessStatusCode();

            var newPlayer = await response.Content.ReadFromJsonAsync<PlayerItem>();
            return newPlayer;
        }

        public async Task<PlayerItem?> Update(int id, PlayerItem playerItem)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/PlayerItem/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.PutAsJsonAsync(route, playerItem);

            response.EnsureSuccessStatusCode();

            var updatedPlayer = await response.Content.ReadFromJsonAsync<PlayerItem>();
            return updatedPlayer;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/PlayerItem/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
