using Newtonsoft.Json;

namespace Tenor
{
    public class MediaObject
    {
		[JsonProperty("dims")]
		public int[] Dims;

		[JsonProperty("duration")]
		public double Duration;

		[JsonProperty("size")]
		public int Size;

		[JsonProperty("url")]
		public Uri Url;
	}
}
