using System.Net.Http.Json;
using ActionCommandGame.Model;

namespace ActionCommandGame.Sdk
{
    public class PlayerSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlayerSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<Player>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/api/Player";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var players = await response.Content.ReadFromJsonAsync<IList<Player>>();
            if (players is null)
            {
                return new List<Player>();
            }
            return players;
        }

        public async Task<Player?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/Player/{id}";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var player = await response.Content.ReadFromJsonAsync<Player>();
            return player;
        }

        public async Task<Player?> Create(Player player)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/Player";
            var response = await httpClient.PostAsJsonAsync(route, player);

            response.EnsureSuccessStatusCode();

            var newPlayer = await response.Content.ReadFromJsonAsync<Player>();
            return newPlayer;
        }

        public async Task<Player?> Update(int id, Player player)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/Player/{id}";
            var response = await httpClient.PostAsJsonAsync(route, player);

            response.EnsureSuccessStatusCode();

            var updatedPlayer = await response.Content.ReadFromJsonAsync<Player>();
            return updatedPlayer;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/Player/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
