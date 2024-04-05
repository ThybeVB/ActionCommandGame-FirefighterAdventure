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
    }
}
