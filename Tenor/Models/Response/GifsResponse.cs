using Newtonsoft.Json;

namespace Tenor
{
    public class GifsResponse
    {
        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("results")]
        public GifObject[] Results { get; set; }
    }
}
