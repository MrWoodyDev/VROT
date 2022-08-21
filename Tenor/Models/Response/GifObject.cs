using Newtonsoft.Json;

namespace Tenor
{
    public class GifObject
    {
        [JsonProperty("created")]
        public double Created { get; set; }

        [JsonProperty("hasaudio")]
        public bool HasAudio { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("media_formats")]
        public Dictionary<FormatType, MediaObject> MediaFormats { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content_descritpion")]
        public string ContentDescription { get; set; }

        [JsonProperty("itemurl")]
        public string ItemUrl { get; set; }

        [JsonProperty("hascaption")]
        public bool HasCaption { get; set; }

        [JsonProperty("flags")]
        public string[] Flags { get; set; }

        [JsonProperty("bg_color")]
        public string BgColor { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
