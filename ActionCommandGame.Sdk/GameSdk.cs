using System.Net.Http.Headers;
using System.Net.Http.Json;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Ui.Mvc.Stores;

namespace ActionCommandGame.Sdk
{
    public class GameSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenStore _tokenStore;

        public GameSdk(IHttpClientFactory httpClientFactory, TokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<GameResult>> PerformAction(string pId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/Game/PerformAction/{pId}";
            var bearer = _tokenStore.GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<GameResult>>();
            if (result is null)
            {
                return new ServiceResult<GameResult>
                {
                    Messages =
                    {
                        new ServiceMessage()
                        {
                            Code = "Error", Message = "Failed to read from json"
                        }
                    }
                };
            }

            return result;
        }

        public async Task<ServiceResult<BuyResult>> Buy(string pId, int iId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/Game/Buy/{pId}/{iId}";
            var bearer = _tokenStore.GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<BuyResult>>();
            if (result is null)
            {
                return new ServiceResult<BuyResult>
                {
                    Messages =
                    {
                        new ServiceMessage()
                        {
                            Code = "Error", Message = "Failed to read from json"
                        }
                    }
                };
            }

            return result;
        }
    }
}
