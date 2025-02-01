using Newtonsoft.Json;

namespace AtechTask.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task<string> GetStringAsync(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
