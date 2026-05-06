using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PixelArtGenerator.Infrastructure
{
    public class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient()
        {
            _http = new HttpClient();
        }

        public async Task<string> GenerateImage(string description, int width = 128, int height = 128)
        {
            var url = "https://api.pixellab.ai/v1/generate-image-pixflux";
            var apiToken = "YOUR_API_TOKEN_HERE";

            var payload = new
            {
                description = description,
                image_size = new
                {
                    width = width,
                    height = height
                }
            };

            var json = JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", $"Bearer {apiToken}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception($"API Error: {response.StatusCode} \n {result}");
            }

            return result;
        }
    }
}