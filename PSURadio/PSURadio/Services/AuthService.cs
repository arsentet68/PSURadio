using PSURadio.Models;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PSURadio.Services
{
    public class AuthService
    {
        const string Url = "http://10.0.2.2:5127/api/auth/";
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<AuthResult> Add(Auth auth)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonSerializer.Serialize(auth),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<AuthResult>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<bool> AuthenticateAndSaveSession(Auth auth)
        {
            AuthResult result = await Add(auth);
            if (result != null)
            {
                SessionService.CurrentAuthResult = result;
/*                MessagingCenter.Send(result, "UserRoleChanged");*/
                return true;
            }
            return false;
        }
    }
}



