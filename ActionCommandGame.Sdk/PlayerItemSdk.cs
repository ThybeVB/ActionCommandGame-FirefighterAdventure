using ActionCommandGame.Model;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class PlayerItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlayerItemSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<PlayerItem>> Find(int? currentPlayerId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/PlayerItem/find";
            if (currentPlayerId.HasValue)
            {
                route = $"/api/PlayerItem/find/{currentPlayerId}";
            }
            
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
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var player = await response.Content.ReadFromJsonAsync<PlayerItem>();
            return player;
        }

        public async Task<PlayerItem?> Create(PlayerItem playerItem)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/PlayerItem";
            var response = await httpClient.PostAsJsonAsync(route, playerItem);

            response.EnsureSuccessStatusCode();

            var newPlayer = await response.Content.ReadFromJsonAsync<PlayerItem>();
            return newPlayer;
        }

        public async Task<PlayerItem?> Update(int id, PlayerItem playerItem)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/PlayerItem/{id}";
            var response = await httpClient.PutAsJsonAsync(route, playerItem);

            response.EnsureSuccessStatusCode();

            var updatedPlayer = await response.Content.ReadFromJsonAsync<PlayerItem>();
            return updatedPlayer;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/PlayerItem/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
