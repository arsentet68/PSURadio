using PSURadio.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSURadio.Services
{
    public class MessageService
    {
        private const string Url = "http://10.0.2.2:5127/api/messages/";
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public MessageService()
        {
            _client = new HttpClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            var response = await _client.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Message>>(content, _options);
            }
            return new List<Message>();
        }

        public async Task<bool> SendMessageAsync(Message message)
        {
            var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(Url, content);
            return response.IsSuccessStatusCode;
        }
    }
}
