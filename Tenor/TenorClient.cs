using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Tenor
{
    public class TenorClient
    {
        private const string BaseUri = "https://tenor.googleapis.com/v2/";

        private string ApiKey { get; }

        private string ClientKey { get; }

        private readonly HttpClient _httpClient;

        public TenorClient(HttpClient httpClient, string apiKey, string clientKey)
        {
            ApiKey = apiKey;
            ClientKey = clientKey;
            _httpClient = httpClient;
        }

        public async Task<GifsResponse> SearchAsync(GetGifsRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var parametersDictionary = request.ToQueryParametersDictionary();

            var response = await _httpClient.GetAsync(BuildUrlQuery("search", parametersDictionary));
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var gifsResponse = JsonConvert.DeserializeObject<GifsResponse>(responseJson);

            return gifsResponse; 
        }

        private string BuildUrlQuery(string endpoint, IDictionary<string, string> queryParameters)
        {
            queryParameters.Add("key", ApiKey);
            queryParameters.Add("client_key", ClientKey);

            return QueryHelpers.AddQueryString(BaseUri + endpoint, queryParameters);
        }
    }
}