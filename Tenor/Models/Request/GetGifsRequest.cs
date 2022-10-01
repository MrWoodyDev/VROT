namespace Tenor;

public class GetGifsRequest
{
    public string SearchQuery { get; set; }

    public string Country { get; set; } = "US";

    public string Locale { get; set; } = "en_US";

    public ContentFilter ContentFilter { get; set; }

    public FormatType[] MediaFilter { get; set; }

    public AspectRatioRange AspectRatioRange { get; set; }

    public bool Random { get; set; }

    public byte Limit { get; set; }

    public string Pos { get; set; }

    public IDictionary<string, string> ToQueryParametersDictionary()
    {
        var parameters = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("q", SearchQuery),
            new KeyValuePair<string, string>("country", Country),
            new KeyValuePair<string, string>("locale", Locale),
            new KeyValuePair<string, string>("content_filter", ContentFilter.ToString() ?? string.Empty),
            new KeyValuePair<string, string>("media_filter", string.Join(',', MediaFilter)),
            new KeyValuePair<string, string>("ar_range", AspectRatioRange.ToString() ?? string.Empty),
            new KeyValuePair<string, string>("random", Random.ToString().ToLower() ?? string.Empty),
            new KeyValuePair<string, string>("limit", Limit.ToString() ?? "20"),
            new KeyValuePair<string, string>("pos", Pos)
        };


        return new Dictionary<string, string>(parameters.Where(p => !string.IsNullOrWhiteSpace(p.Value)));
    }
}