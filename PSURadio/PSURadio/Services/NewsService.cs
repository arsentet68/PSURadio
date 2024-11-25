using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PSURadio.Models;

namespace PSURadio.Services
{
    public class NewsService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public NewsService()
        {
            _client = new HttpClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<News>> GetNewsAsync()
        {
            var response = await _client.GetStringAsync("http://10.0.2.2:5127/api/news");
            return JsonSerializer.Deserialize<List<News>>(response, _options);
        }
        public async Task UpdateNewsAsync(News news)
        {
            var url = $"http://10.0.2.2:5127/api/news/{news.Id}";
            var json = JsonSerializer.Serialize(news, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update news: {response.StatusCode}, {errorContent}");
            }
        }
        public async Task DeleteNewsAsync(int id)
        {
            var response = await _client.DeleteAsync($"http://10.0.2.2:5127/api/news/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception($"Failed to delete news: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        public async Task AddNewsAsync(News news)
        {
            var url = "http://10.0.2.2:5127/api/news";
            var json = JsonSerializer.Serialize(news, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to add news: {response.StatusCode}, {errorContent}");
            }
        }
    }
}

