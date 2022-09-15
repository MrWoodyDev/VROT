using Newtonsoft.Json;

namespace VROT.Models
{
    public partial class Event
    {
        [JsonProperty("activity")]
        public string? Activity { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("participants")]
        public long Participants { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("link")]
        public string? Link { get; set; }

        [JsonProperty("interaction")]
        public string? Interaction { get; set; }

        [JsonProperty("accessibility")]
        public double Accessibility { get; set; }

        [JsonProperty("mod")]
        public string? Mod { get; set; }

        [JsonProperty("fun")]
        public string? Fun { get; set; }
    }

    public partial class Event
    {
        public static Event FromJson(string json) => JsonConvert.DeserializeObject<Event>(json, Converter.Settings);
    }
}
