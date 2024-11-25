using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSURadio.Services
{
    public class RegisterService
    {
        const string Url = "http://10.0.2.2:5127/api/users/register/";
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public RegisterService()
        {
            _client = new HttpClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<bool> RegisterUser(string username, string email, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Username = username, Email = email, Password = password }), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(Url, content);
            return response.IsSuccessStatusCode;
        }
    }
}

