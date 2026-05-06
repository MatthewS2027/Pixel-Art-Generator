using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PixelArtGenerator.Infrastructure
{
    public class ApiClient
    {

        // Create class instance and call constructor
        private readonly HttpClient _http;

        public ApiClient()
        {
            _http = new HttpClient();
        }
        // -------------------------


        public async Task<string> GenerateImage(string description, int width = 128, int height = 128)
        {
            // url holds PixelLab's specific endpoint for their "Pixflux" image generation model
            var url = "https://api.pixellab.ai/v1/generate-image-pixflux";
            // The API token is a secret key that proves your app is authorized to use their service
            var apiToken = "YOUR_API_TOKEN_HERE";

            // ----------------------------------------
            // This section of the method create and serialize the payload into a JSON file

            // the payload is an anonymous object that will become the request body
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
            // ----------------------------------------

            // ----------------------------------------
            // This section of the method will create and send the HTTP request

            var request = new HttpRequestMessage(HttpMethod.Post, url);     // POST is an http method used to send information to a specified source
            request.Headers.Add("Authorization", $"Bearer {apiToken}");     // REST API verifies identity

            //The JSON string is sent as the request body, encoded in UTF8, and labeled as 'application/json'
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");       


            // http.SencAsync fires the request and then waits asynchronously so that app stays responsive
            var response = await _http.SendAsync(request);

            // -----------------------------------------

            // -----------------------------------------
            // This section of code will deal with handling the response

            var result = await response.Content.ReadAsStringAsync();    // The response body is read as a raw string. JSON of generated image data

            // Checks for errors with response
            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception($"API Error: {response.StatusCode} \n {result}");
            }

            return result;

            // -----------------------------------------
            
        }
    }
}