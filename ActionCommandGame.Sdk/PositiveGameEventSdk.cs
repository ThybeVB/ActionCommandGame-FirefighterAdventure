﻿using ActionCommandGame.Model;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class PositiveGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PositiveGameEventSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<PositiveGameEvent>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = "/api/PositiveGameEvent";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var positiveGameEvents = await response.Content.ReadFromJsonAsync<IList<PositiveGameEvent>>();
            if (positiveGameEvents is null)
            {
                return new List<PositiveGameEvent>();
            }
            return positiveGameEvents;
        }

        public async Task<PositiveGameEvent?> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/PositiveGameEvent/{id}";
            var response = await httpClient.GetAsync(route);

            response.EnsureSuccessStatusCode();

            var positiveGameEvent = await response.Content.ReadFromJsonAsync<PositiveGameEvent>();
            return positiveGameEvent;
        }

        public async Task<PositiveGameEvent?> Create(PositiveGameEvent request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/PositiveGameEvent";
            var response = await httpClient.PostAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var positiveGameEvent = await response.Content.ReadFromJsonAsync<PositiveGameEvent>();
            return positiveGameEvent;
        }

        public async Task<PositiveGameEvent?> Update(int id, PositiveGameEvent request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"api/PositiveGameEvent/{id}";
            var response = await httpClient.PutAsJsonAsync(route, request);

            response.EnsureSuccessStatusCode();

            var updatedEvent = await response.Content.ReadFromJsonAsync<PositiveGameEvent>();
            return updatedEvent;
        }

        public async Task Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var route = $"/api/PositiveGameEvent/{id}";
            var response = await httpClient.DeleteAsync(route);

            response.EnsureSuccessStatusCode();
        }
    }
}