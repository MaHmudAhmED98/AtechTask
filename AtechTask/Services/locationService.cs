using AtechTask.IServices;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace AtechTask.Services
{
    public class locationService : IlocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public locationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["locationApi:ApiKey"];
        }

        public async Task<string> GetCountryCodeFromIpAsync(string ipAddress)
        {
            using HttpRequestMessage requestMessage = new HttpRequestMessage
                ( HttpMethod.Get, $"https://api.ipgeolocation.io/ipgeo?apiKey={_apiKey}&ip={ipAddress}");

            HttpResponseMessage responseMessage = await this._httpClient.SendAsync(requestMessage);
            string responseStr = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseStr);
            }
            var json = JObject.Parse(responseStr);
            return json["country_code2"]?.ToString();

        }
    }
}
