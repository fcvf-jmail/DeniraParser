using Newtonsoft.Json;

namespace DeniraParser;

public class BotInfoResponse
{
    [JsonProperty("ok")]
    public required bool Ok { get; set; }

    [JsonProperty("result")]
    public required BotInfo Result { get; set; }
}