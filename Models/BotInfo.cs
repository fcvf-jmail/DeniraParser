using Newtonsoft.Json;

namespace DeniraParser;

public class BotInfo
{
    [JsonProperty("id")]
    public required long Id { get; set; }

    [JsonProperty("first_name")]
    public required string FirstName { get; set; }

    [JsonProperty("username")]
    public required string UserName { get; set; }
}
