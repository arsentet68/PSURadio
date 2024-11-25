using PSURadio.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSURadio.Services
{
    public class ProfileEditService
    {
        const string Url = "http://10.0.2.2:5127/api/users/";
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public ProfileEditService()
        {
            _client = new HttpClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<bool> UpdateUserName(string userId, string newUserName)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { UserName = newUserName }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{Url}{userId}/username", content);
            return response.IsSuccessStatusCode;
        }
        // Метод для получения пользователей с определенной ролью
        public async Task<List<User>> GetUsersByRoleAsync(int role)
        {
            var response = await _client.GetAsync($"{Url}role/{role}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<User>>(json, _options);
            }
            return new List<User>();
        }
        // Метод для изменения роли пользователя
        public async Task<bool> UpdateUserRole(string userId, int newRole)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Role = newRole }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{Url}{userId}/role", content);
            return response.IsSuccessStatusCode;
        }
        // Добавьте аналогичные методы для изменения других данных профиля, если необходимо
    }
}

