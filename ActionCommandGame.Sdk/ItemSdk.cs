using ActionCommandGame.Model;
using System.Net.Http.Json;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Sdk.Extensions;

namespace ActionCommandGame.Sdk
{
    public class ItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public ItemSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<IList<Item>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = "/api/Item";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var items = await response.Content.ReadFromJsonAsync<IList<Item>>();
            if (items is null)
            {
                return new List<Item>();
            }
            return items;
        }

        public async Task<Item?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"/api/Item/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var item = await response.Content.ReadFromJsonAsync<Item>();
            return item;
        }

        public async Task<Item?> Create(Item item)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"api/Item";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.PostAsJsonAsync(route, item);

            response.EnsureSuccessStatusCode();

            var newItem = await response.Content.ReadFromJsonAsync<Item>();
            return newItem;
        }

        public async Task<Item?> Update(int id, Item item)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"api/Item/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.PutAsJsonAsync(route, item);

            response.EnsureSuccessStatusCode();

            var updatedItem = await response.Content.ReadFromJsonAsync<Item>();
            return updatedItem;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientExtensions.ApiName);
            var route = $"/api/Item/{id}";
            var token = _tokenStore.GetToken();
            httpClient.AddAuthorization(token);
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}
