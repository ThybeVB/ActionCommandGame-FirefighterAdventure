using ActionCommandGame.Model;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class NegativeGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NegativeGameEventSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<NegativeGameEvent>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/api/NegativeGameEvent";
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
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/NegativeGameEvent/{id}";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var negativeGameEvent = await response.Content.ReadFromJsonAsync<NegativeGameEvent>();
            return negativeGameEvent;
        }

        public async Task<NegativeGameEvent?> Create(NegativeGameEvent request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/NegativeGameEvent";
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var negativeGameEvent = await response.Content.ReadFromJsonAsync<NegativeGameEvent>();
            return negativeGameEvent;
        }

        public async Task<NegativeGameEvent?> Update(int id, NegativeGameEvent request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/NegativeGameEvent/{id}";
            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var updatedEvent = await response.Content.ReadFromJsonAsync<NegativeGameEvent>();
            return updatedEvent;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/NegativeGameEvent/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
