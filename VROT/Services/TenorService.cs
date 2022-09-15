using Microsoft.Extensions.Options;
using Tenor;
using VROT.Models;

namespace VROT.Services
{
    internal class TenorService : ITenorService
    {
        private readonly TenorClient _tenorClient;

        private readonly IHttpClientFactory _httpClientFactory;

        public TenorService(IHttpClientFactory httpClientFactory, IOptions<TenorSetttings> options)
        {
            _httpClientFactory = httpClientFactory;
            _tenorClient = new TenorClient(_httpClientFactory.CreateClient(), options.Value.ApiKey, options.Value.ClientKey);
        }

        public async Task<string?> GetRandomGifUrlAsync(string search)
        {
            int randaomGif = 0;
            Random random = new Random();
            randaomGif = random.Next(0, 49);

            var gifsRequest = GetDefaultGifsRequest();
            gifsRequest.SearchQuery = search;
            gifsRequest.Random = true;
            gifsRequest.Limit = 50;

            var result = await _tenorClient.SearchAsync(gifsRequest);

            if (result is null)
                return null;

            return result.Results[randaomGif].MediaFormats[FormatType.gif].Url.ToString() ?? null;
        }


        private GetGifsRequest GetDefaultGifsRequest() =>
            new GetGifsRequest
            {
                ContentFilter = ContentFilter.off,
                AspectRatioRange = AspectRatioRange.all,
                MediaFilter = new FormatType[] { FormatType.gif, FormatType.mediumgif },
                Limit = 20,
                Pos = "0"
            };
    }
}
