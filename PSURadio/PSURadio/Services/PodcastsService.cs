using PSURadio.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSURadio.Services
{
    public class PodcastsService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public PodcastsService()
        {
            _client = new HttpClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<Podcast>> GetPodcastsAsync()
        {
            var response = await _client.GetStringAsync("http://10.0.2.2:5127/api/podcasts");
            return JsonSerializer.Deserialize<List<Podcast>>(response, _options);
        }
        public async Task UpdatePodcastAsync(Podcast podcast)
        {
            var url = $"http://10.0.2.2:5127/api/podcasts/{podcast.Id}";
            var json = JsonSerializer.Serialize(podcast, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update podcast: {response.StatusCode}, {errorContent}");
            }
        }
        public async Task DeletePodcastAsync(int id)
        {
            var response = await _client.DeleteAsync($"http://10.0.2.2:5127/api/podcasts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception($"Failed to delete news: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        public async Task AddPodcastAsync(Podcast podcast)
        {
            var url = "http://10.0.2.2:5127/api/podcasts";
            var json = JsonSerializer.Serialize(podcast, _options);
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
