using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PSURadio.Models;

namespace PSURadio.Services
{
    public class PlaylistsService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public PlaylistsService()
        {
            _client = new HttpClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<Playlist>> GetPlaylistsAsync()
        {
            var response = await _client.GetStringAsync("http://10.0.2.2:5127/api/playlists");
            return JsonSerializer.Deserialize<List<Playlist>>(response, _options);
        }
        public async Task UpdatePlaylistAsync(Playlist playlist)
        {
            var url = $"http://10.0.2.2:5127/api/playlists/{playlist.Id}";
            var json = JsonSerializer.Serialize(playlist, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update playlist: {response.StatusCode}, {errorContent}");
            }
        }
        public async Task DeletePlaylistAsync(int id)
        {
            var response = await _client.DeleteAsync($"http://10.0.2.2:5127/api/playlists/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception($"Failed to delete playlist: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        public async Task AddPlaylistAsync(Playlist playlist)
        {
            var url = "http://10.0.2.2:5127/api/playlists";
            var json = JsonSerializer.Serialize(playlist, _options);
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

