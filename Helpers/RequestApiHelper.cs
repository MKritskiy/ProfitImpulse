using System.Text.Json.Nodes;
using System.Text.Json;
using System.Net.Http.Json;

namespace Helpers
{
    public class RequestApiHelper : IRequestApiHelper
    {
        IHttpClientFactory _httpClientFactory;

        public RequestApiHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Fetch api dto model from api with authkey
        public async Task<IEnumerable<T>> FetchListFromApi<T>(string query, int profileid, string jwtToken)
        {
            using var client = _httpClientFactory.CreateClient();
            // Get auth key
            string? authkey = await GetAuthorizationKey(profileid, jwtToken) ?? null;

            if (authkey == null)
                throw new Exception("Authorization key not found.");

            // Add auth key in header params
            client.DefaultRequestHeaders.Add("Authorization", authkey);

            // Get apiObject from WB api
            var responce = await client.GetAsync(query);
            responce.EnsureSuccessStatusCode();
            var content = await responce.Content.ReadAsStringAsync();
            var apiObject = JsonSerializer.Deserialize<IEnumerable<T>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiObject == null)
                throw new Exception("Failed to fetch Objects from api.");

            return apiObject;

        }

        public async Task<string?> GetAuthorizationKey(int profileid, string jwtToken)
        {
            using var client = _httpClientFactory.CreateClient();
            var url = HelperConstants.ProfileAuthKeyQuery + profileid;
            try
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonObject>();
                    return result?["apiKey"]?.ToString();
                }
                else
                {
                    Console.WriteLine($"Error: Received {response.StatusCode} for URL {url}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                // Log the exception message and the URL
                Console.WriteLine($"HttpRequestException: {e.Message} for URL {url}");
                return null;
            }
        }
    }
}
