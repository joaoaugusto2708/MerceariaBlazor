using System.Text.Json;

namespace Mercearia.UI.Client.Helpers
{
    public class Api
    {
        private readonly string enderecoBase;
        private readonly HttpClient httpClient;
        public Api(string enderecoBase)
        { 
            this.enderecoBase = enderecoBase;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(enderecoBase);
        }

        public async Task<IList<T>> GetAsync<T>(string url) where T : new()
        {
            using HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<IList<T>>(responseBody, options) ?? new List<T>();
        }

        public async Task<T?> GetTAsync<T>(string url, string id) where T : new()
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{url}/{id}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T?>(responseBody, options);
        }

        public async Task<T?> PostAsync<T>(string url, T objeto) where T : new()
        {
            using StringContent stringContent = new(
                JsonSerializer.Serialize(objeto),
                System.Text.Encoding.UTF8,
                "application/json"
           );
            using HttpResponseMessage response = await httpClient.PostAsync(url, stringContent);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T?>(responseBody, options);
        }

        public async Task PutAsync<T>(string url, string id, T objeto) where T: new() 
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(objeto),
                System.Text.Encoding.UTF8,
                "application/json"
                );
            using HttpResponseMessage response = await httpClient.PutAsync($"{url}/{id}", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string url, string id) 
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync($"{url}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
